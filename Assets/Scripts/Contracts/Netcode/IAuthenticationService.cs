
using System.Threading;
using System.Threading.Tasks;

public interface IAuthenticationService : IAuthenticationContext
{
    public Task LoginAsync(string email, string password, CancellationToken ct);
    public Task LogoutAsync(CancellationToken ct);
    public Task<bool> TryRestoreSessionAsync(CancellationToken ct);
}
