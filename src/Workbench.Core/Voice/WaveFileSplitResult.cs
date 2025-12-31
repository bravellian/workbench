namespace Workbench.Core.Voice;

/// <summary>
/// Result payload for WAV file splitting.
/// </summary>
/// <param name="Chunks">Paths to WAV chunk files.</param>
/// <param name="HasTempFiles">True when temporary files were created.</param>
public sealed record WaveFileSplitResult(IReadOnlyList<string> Chunks, bool HasTempFiles);
