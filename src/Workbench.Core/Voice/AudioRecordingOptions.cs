using Workbench.VoiceViz;

namespace Workbench.Core.Voice;

/// <summary>
/// Options controlling audio recording and output.
/// </summary>
/// <param name="Format">PCM format for capture.</param>
/// <param name="MaxDuration">Maximum recording duration.</param>
/// <param name="MaxBytes">Maximum size in bytes before forcing stop.</param>
/// <param name="OutputDirectory">Directory for temporary audio files.</param>
/// <param name="FilePrefix">Prefix for output file names.</param>
/// <param name="FramesPerBuffer">Frames per buffer for the recorder.</param>
/// <param name="Tap">Optional audio tap for visualization.</param>
public sealed record AudioRecordingOptions(
    AudioFormat Format,
    TimeSpan MaxDuration,
    long MaxBytes,
    string OutputDirectory,
    string FilePrefix,
    uint FramesPerBuffer,
    IAudioTap? Tap = null);
