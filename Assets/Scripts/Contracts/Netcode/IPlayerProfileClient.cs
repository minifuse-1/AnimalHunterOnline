using System.Threading;
using System.Threading.Tasks;

public interface IPlayerProfileClient
{
    public Task<PlayerProfile> GetMyProfileAsync(AccessToken accessToken, CancellationToken ct);
}
