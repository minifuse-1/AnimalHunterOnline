using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IAuthenticationSessionStore
{
    public Task<AuthenticationResult?> LoadAsync(CancellationToken ct);
    public Task SaveAsync(AuthenticationResult authResult, CancellationToken ct);
    public Task ClearAsync(CancellationToken ct);
}
