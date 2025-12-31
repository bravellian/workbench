// Voice capture entrypoints for CLI commands.
// Manages a short-lived recording session, transcription, and temp file cleanup.
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Workbench.Core;
using Workbench.Core.Voice;

namespace Workbench.Cli;

public partial class Program
{
    static async Task<string?> CaptureVoiceTranscriptAsync(VoiceConfig config, CancellationToken ct)
    {
        var limits = AudioLimiter.Calculate(config.Format, config.MaxDuration, config.MaxUploadBytes);
        var options = new AudioRecordingOptions(
            config.Format,
            limits.MaxDuration,
            limits.MaxBytes,
            Path.GetTempPath(),
            "workbench-voice",
            FramesPerBuffer: 512);

        var recorder = new PortAudioRecorder();
        var session = await recorder.StartAsync(options, ct).ConfigureAwait(false);
        await using (session.ConfigureAwait(false))
        {
            Console.WriteLine("Recording... Press ENTER to stop. Press ESC to cancel.");

            while (!session.Completion.IsCompleted)
            {
                if (!Console.IsInputRedirected && Console.KeyAvailable)
                {
                    // Polling avoids blocking the recorder loop while still allowing quick cancel/stop.
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        await session.StopAsync(ct).ConfigureAwait(false);
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        await session.CancelAsync(ct).ConfigureAwait(false);
                    }
                }
                await Task.Delay(50, ct).ConfigureAwait(false);
            }

            var recording = await session.Completion.ConfigureAwait(false);
            if (recording.WasCanceled)
            {
                Console.WriteLine("Recording canceled.");
                return null;
            }

            if (!OpenAiTranscriptionClient.TryCreate(out var transcriptionClient, out var reason))
            {
                Console.WriteLine($"Transcription disabled: {reason}");
                CleanupTempFiles(recording.WavPaths);
                return null;
            }

            var transcript = string.Empty;
            var tempFiles = new List<string>();
            try
            {
                var transcriber = new VoiceTranscriptionService(transcriptionClient!, config);
                var result = await transcriber.TranscribeAsync(recording, ct).ConfigureAwait(false);
                transcript = result.Transcript.Trim();
                tempFiles.AddRange(result.TempFiles);
            }
            finally
            {
                // Best-effort cleanup to avoid leaving temporary audio on disk.
                CleanupTempFiles(recording.WavPaths);
                CleanupTempFiles(tempFiles);
            }

            if (string.IsNullOrWhiteSpace(transcript))
            {
                Console.WriteLine("Transcription returned no text.");
                return null;
            }

            return transcript;
        }
    }
}
