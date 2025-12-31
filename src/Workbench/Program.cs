// Entry point that routes to TUI or CLI based on first argument.
// Keeps the top-level binary stable while allowing multiple front-ends.
namespace Workbench;

public static class Program
{
    /// <summary>
    /// Main entry point for the Workbench binary.
    /// </summary>
    /// <param name="args">Command-line arguments passed from the host process.</param>
    /// <returns>Exit code.</returns>
    public static Task<int> Main(string[] args)
    {
        if (args.Length > 0)
        {
            var command = args[0];
            if (string.Equals(command, "tui", StringComparison.OrdinalIgnoreCase)
                || string.Equals(command, "t", StringComparison.OrdinalIgnoreCase))
            {
                var tuiArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();
                return Workbench.Tui.TuiEntrypoint.RunAsync(tuiArgs);
            }
        }

        return Workbench.Cli.Program.RunAsync(args);
    }
}
