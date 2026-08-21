using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class AuthenticationService : IAuthenticationService
{
    public bool IsAuthenticated => _currentAuth.HasValue;
    private AuthenticationResult? _currentAuth;
    private readonly IAuthenticationClient _authClient;
    private readonly IPlayerProfileClient _profileClient;
    private readonly IAuthenticationSessionStore _authSessionStore;
    private readonly IPlayerProfileSession _profileSession;
    public AuthenticationService(
        IAuthenticationClient authClient, 
        IPlayerProfileClient profileClient, 
        IAuthenticationSessionStore authenticationSessionStore, 
        IPlayerProfileSession profileSession)
    {
        _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        _profileClient = profileClient ?? throw new ArgumentNullException(nameof(profileClient));
        _authSessionStore = authenticationSessionStore ?? throw new ArgumentNullException(nameof(authenticationSessionStore));
        _profileSession = profileSession ?? throw new ArgumentNullException(nameof(profileSession));
    }

    public async Task LoginAsync(string email, string password, CancellationToken ct)
    {
        AuthenticationResult auth = await _authClient.LoginAsync(email, password, ct);
        PlayerProfile profile = await _profileClient.GetMyProfileAsync(auth.AccessToken, ct);
        await _authSessionStore.SaveAsync(auth, ct);
        ct.ThrowIfCancellationRequested();
        _currentAuth = auth;
        _profileSession.SetCurrentProfile(profile);
    }

    public async Task LogoutAsync(CancellationToken ct)
    {
        await _authSessionStore.ClearAsync(ct);
        _currentAuth = null;
        _profileSession.ClearCurrentProfile();
    }

    public bool TryGetAccessToken(out AccessToken accessToken)
    {
        if (_currentAuth.HasValue)
        {
            accessToken = _currentAuth.Value.AccessToken;
            return true;
        }

        accessToken = default;
        return false;
    }

    public async Task<bool> TryRestoreSessionAsync(CancellationToken ct)
    {
        AuthenticationResult? storedAuth = await _authSessionStore.LoadAsync(ct);

        if (!storedAuth.HasValue) return false;

        try
        {
            PlayerProfile profile = await _profileClient.GetMyProfileAsync(storedAuth.Value.AccessToken, ct);
            ct.ThrowIfCancellationRequested();
            _currentAuth = storedAuth.Value;
            _profileSession.SetCurrentProfile(profile);
            return true;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            Debug.Log("Unauthorized access error.");
            await _authSessionStore.ClearAsync(ct);
            _currentAuth = null;
            _profileSession.ClearCurrentProfile();
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }
    }
}
