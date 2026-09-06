using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class JsonMatchResultSink : IMatchResultSink
{
    private readonly string _path;

    public JsonMatchResultSink(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException(nameof(path));
        _path = path;
    }

    public async Task HandleResultAsync(MatchResult matchResult, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        MatchHistoryDTO history = await LoadMatchHistory(ct);
        history.results ??= new List<MatchResultDTO>();
        MatchResultDTO result = matchResult.ToDTO();
        history.results.Add(result);
        string json = JsonUtility.ToJson(history, prettyPrint: true);
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
        Debug.Log($"Updated Match history at {_path}");
    }

    private async Task<MatchHistoryDTO> LoadMatchHistory(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (!File.Exists(_path)) return new MatchHistoryDTO
        {
            version = AppConstants.MatchHistory.FormatVersion,
            results = new List<MatchResultDTO>()
        };

        using FileStream stream = new(
            _path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true
        );

        using StreamReader reader = new (stream, Encoding.UTF8);

        string json = await reader.ReadToEndAsync();
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(json)) return new MatchHistoryDTO
        {
            version = AppConstants.MatchHistory.FormatVersion,
            results = new List<MatchResultDTO>()
        };

        MatchHistoryDTO history = JsonUtility.FromJson<MatchHistoryDTO>(json) ?? throw new InvalidDataException(nameof(json));

        if (history.version != AppConstants.MatchHistory.FormatVersion) throw new FormatException($"MatchHistoryDTO file format incorrect: {history.version}");

        history.results ??= new List<MatchResultDTO>();
        return history;
    }
}
