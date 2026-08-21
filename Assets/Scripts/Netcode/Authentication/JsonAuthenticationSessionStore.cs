using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class JsonAuthenticationSessionStore : IAuthenticationSessionStore
{
    private string _path;
    public JsonAuthenticationSessionStore(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));
        _path = path;
    }
    public Task ClearAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (File.Exists(_path))
            File.Delete(_path);

        return Task.CompletedTask;
    }

    public async Task<AuthenticationResult?> LoadAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!File.Exists(_path)) return null;

        using FileStream stream = new(
            _path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true
            );

        using StreamReader reader = new(stream, Encoding.UTF8);
        string json = await reader.ReadToEndAsync();
        ct.ThrowIfCancellationRequested();
        AuthenticationSessionDto dto = JsonUtility.FromJson<AuthenticationSessionDto>(json);

        if (dto == null || string.IsNullOrWhiteSpace(dto.accessToken)) return null;

        Debug.Log($"Successfully Loaded Authentication token from {_path}");

        return new AuthenticationResult(new AccessToken(dto.accessToken));
    }

    public async Task SaveAsync(AuthenticationResult authResult, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(authResult.AccessToken.Value)) throw new ArgumentException(nameof(authResult.AccessToken.Value));

        AuthenticationSessionDto dto = new()
        {
            accessToken = authResult.AccessToken.Value
        };

        string json = JsonUtility.ToJson(dto, prettyPrint: true);
        ct.ThrowIfCancellationRequested();
        using FileStream stream = new(
            _path,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true
        );

        using StreamWriter writer = new(stream, Encoding.UTF8);

        await writer.WriteAsync(json.AsMemory());

        Debug.Log($"Successfully saved Accesstoken to {_path}");
    }
}
