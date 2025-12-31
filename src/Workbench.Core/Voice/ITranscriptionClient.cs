namespace Workbench.Core.Voice;

public interface ITranscriptionClient
{
    Task<string> TranscribeAsync(string wavPath, CancellationToken ct);
}
