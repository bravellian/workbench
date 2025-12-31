namespace Workbench.Core.Voice;

/// <summary>
/// Result payload for a completed audio recording session.
/// </summary>
/// <param name="WavPaths">Paths to generated WAV files.</param>
/// <param name="Duration">Recorded duration.</param>
/// <param name="Format">Audio format used for capture.</param>
/// <param name="BytesWritten">Total bytes written across files.</param>
/// <param name="WasCanceled">True when recording was canceled.</param>
public sealed record AudioRecordingResult(
    IReadOnlyList<string> WavPaths,
    TimeSpan Duration,
    AudioFormat Format,
    long BytesWritten,
    bool WasCanceled);
