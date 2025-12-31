namespace Workbench.Core.Voice;

public sealed record AudioRecordingResult(
    IReadOnlyList<string> WavPaths,
    TimeSpan Duration,
    AudioFormat Format,
    long BytesWritten,
    bool WasCanceled);
