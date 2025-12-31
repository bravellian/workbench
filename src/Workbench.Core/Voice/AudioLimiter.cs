namespace Workbench.Core.Voice;

public static class AudioLimiter
{
    public static AudioRecordingLimits Calculate(AudioFormat format, TimeSpan maxDuration, long maxBytes)
    {
        var bytesPerSecond = format.BytesPerSecond;
        if (bytesPerSecond <= 0)
        {
            return new AudioRecordingLimits(TimeSpan.Zero, 0, 0);
        }

        var durationByBytes = maxBytes > 0
            ? TimeSpan.FromSeconds(maxBytes / (double)bytesPerSecond)
            : maxDuration;

        var effectiveDuration = SelectShorterPositive(maxDuration, durationByBytes);
        if (effectiveDuration <= TimeSpan.Zero)
        {
            return new AudioRecordingLimits(TimeSpan.Zero, 0, 0);
        }

        var maxFrames = (long)Math.Floor(effectiveDuration.TotalSeconds * format.SampleRateHz);
        var effectiveBytes = (long)Math.Floor(effectiveDuration.TotalSeconds * bytesPerSecond);
        return new AudioRecordingLimits(effectiveDuration, effectiveBytes, maxFrames);
    }

    private static TimeSpan SelectShorterPositive(TimeSpan first, TimeSpan second)
    {
        if (first <= TimeSpan.Zero)
        {
            return second;
        }
        if (second <= TimeSpan.Zero)
        {
            return first;
        }
        return first <= second ? first : second;
    }
}
