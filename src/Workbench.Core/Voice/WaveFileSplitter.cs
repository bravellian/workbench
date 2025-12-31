namespace Workbench.Core.Voice;

public static class WaveFileSplitter
{
    public static async Task<WaveFileSplitResult> SplitAsync(
        string wavPath,
        TimeSpan chunkDuration,
        CancellationToken ct)
    {
        var info = WaveFileInfo.Read(wavPath);
        if (chunkDuration <= TimeSpan.Zero || info.Duration <= chunkDuration)
        {
            return new WaveFileSplitResult(new[] { wavPath }, false);
        }

        var bytesPerChunk = (long)(chunkDuration.TotalSeconds * info.BytesPerSecond);
        bytesPerChunk = Math.Max(bytesPerChunk, info.BlockAlign);
        bytesPerChunk -= bytesPerChunk % info.BlockAlign;

        if (bytesPerChunk <= 0 || info.DataLength <= bytesPerChunk)
        {
            return new WaveFileSplitResult(new[] { wavPath }, false);
        }

        var outputDir = Path.GetDirectoryName(wavPath) ?? Path.GetTempPath();
        var baseName = Path.GetFileNameWithoutExtension(wavPath);
        var chunks = new List<string>();

        var stream = File.OpenRead(wavPath);
        await using (stream.ConfigureAwait(false))
        {
            stream.Seek(info.DataOffset, SeekOrigin.Begin);

            long remaining = info.DataLength;
            var buffer = new byte[64 * 1024];
            var chunkIndex = 0;

            while (remaining > 0)
            {
                ct.ThrowIfCancellationRequested();
                chunkIndex++;
                var chunkPath = Path.Combine(outputDir, $"{baseName}-chunk-{chunkIndex:000}.wav");
                chunks.Add(chunkPath);

                using var writer = new WaveFileWriter(chunkPath, new AudioFormat(info.SampleRateHz, info.Channels, info.BitsPerSample));
                var chunkRemaining = Math.Min(bytesPerChunk, remaining);

                while (chunkRemaining > 0)
                {
                    ct.ThrowIfCancellationRequested();
                    var read = await stream.ReadAsync(buffer.AsMemory(0, (int)Math.Min(buffer.Length, chunkRemaining)), ct)
                        .ConfigureAwait(false);
                    if (read <= 0)
                    {
                        break;
                    }
                    writer.WriteBytes(buffer.AsSpan(0, read));
                    chunkRemaining -= read;
                    remaining -= read;
                }
            }

            return new WaveFileSplitResult(chunks, true);
        }
    }
}
