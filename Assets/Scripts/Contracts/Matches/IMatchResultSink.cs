using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface IMatchResultSink
{
    public Task HandleResultAsync(MatchResult matchResult, CancellationToken ct);
}
