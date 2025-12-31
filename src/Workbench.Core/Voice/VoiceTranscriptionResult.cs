namespace Workbench.Core.Voice;

/// <summary>
/// Result payload for a transcription operation.
/// </summary>
/// <param name="Transcript">Transcript text.</param>
/// <param name="TempFiles">Temporary files created during transcription.</param>
public sealed record VoiceTranscriptionResult(string Transcript, IReadOnlyList<string> TempFiles);
