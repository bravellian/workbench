namespace Workbench.Core.Voice;

internal static class VoiceLog
{
    private static readonly Lazy<bool> isDebugEnabled = new(() =>
    {
        var level = Environment.GetEnvironmentVariable("WORKBENCH_LOG_LEVEL");
        if (!string.IsNullOrWhiteSpace(level)
            && level.Trim().Equals("debug", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        var debug = Environment.GetEnvironmentVariable("WORKBENCH_DEBUG");
        return string.Equals(debug, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(debug, "true", StringComparison.OrdinalIgnoreCase);
    });

    public static void Debug(string message)
    {
        if (!isDebugEnabled.Value)
        {
            return;
        }
        Console.Error.WriteLine($"[voice] {message}");
    }
}
