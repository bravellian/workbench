namespace Workbench.Core.Voice;

public sealed record VoiceTranscriptionResult(string Transcript, IReadOnlyList<string> TempFiles);
