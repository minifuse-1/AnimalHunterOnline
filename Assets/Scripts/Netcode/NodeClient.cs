using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NodeClient : IAuthenticationClient, IPlayerProfileClient
{
    private readonly string _baseUrl;

    public NodeClient(string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentException(nameof(baseUrl));
        _baseUrl = baseUrl;
    }
    public async Task<PlayerProfile> GetMyProfileAsync(AccessToken accessToken, CancellationToken ct)
    {
        using UnityWebRequest request = UnityWebRequest.Get(_baseUrl+AppConstants.Api.ProfileEndpoint);
        request.SetRequestHeader("Authorization", $"Bearer {accessToken.Value}");
        await SendAsync(request, ct);

        if (request.responseCode == (long) HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException("Authentication Rejected");
        if (request.result != UnityWebRequest.Result.Success)
            throw new InvalidOperationException();

        PlayerProfileResponseDto dto = JsonUtility.FromJson<PlayerProfileResponseDto>(request.downloadHandler.text);
        return dto.FromDto();
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password, CancellationToken ct)
    {
        LoginRequestDto dto = new() { email = email, password = password };

        string json = JsonUtility.ToJson(dto);

        using UnityWebRequest request = new(_baseUrl + AppConstants.Api.LoginEndpoint, UnityWebRequest.kHttpVerbPOST);
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        await SendAsync(request, ct);

        if (request.responseCode == (long)HttpStatusCode.Unauthorized) throw new UnauthorizedAccessException();
        if (request.result != UnityWebRequest.Result.Success)
            throw new InvalidOperationException($"Request failed with code {request.responseCode}: {request.downloadHandler.text}");

        LoginResponseDto responseDto = JsonUtility.FromJson<LoginResponseDto>(request.downloadHandler.text);

        if (responseDto == null || string.IsNullOrWhiteSpace(responseDto.accessToken)) throw new FormatException($"Login Response Doesn't contain an AccessToken");

        return new AuthenticationResult(new AccessToken(responseDto.accessToken));
    }

    private static async Task SendAsync(UnityWebRequest request, CancellationToken ct)
    {
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            if (ct.IsCancellationRequested)
            {
                request.Abort();
                ct.ThrowIfCancellationRequested();
            }
            await Task.Yield();
        }

        ct.ThrowIfCancellationRequested();
    }
}
