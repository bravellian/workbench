using System.Diagnostics;

namespace Workbench.Core;

public static class CodexService
{
    public sealed record CommandResult(int ExitCode, string StdOut, string StdErr);

    public static CommandResult Run(string repoRoot, params string[] args)
    {
        var psi = new ProcessStartInfo("codex")
        {
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start codex.");
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        return new CommandResult(process.ExitCode, stdout.Trim(), stderr.Trim());
    }

    public static bool TryGetVersion(string repoRoot, out string? version, out string? error)
    {
        version = null;
        error = null;
        try
        {
            var result = Run(repoRoot, "--version");
            if (result.ExitCode != 0)
            {
                error = result.StdErr.Length > 0 ? result.StdErr : "codex --version failed.";
                return false;
            }

            version = result.StdOut;
            return !string.IsNullOrWhiteSpace(version);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public static void StartFullAuto(string repoRoot, string prompt)
    {
        var psi = new ProcessStartInfo("codex")
        {
            WorkingDirectory = repoRoot,
            UseShellExecute = false
        };
        psi.ArgumentList.Add("--full-auto");
        psi.ArgumentList.Add("--web-search");
        psi.ArgumentList.Add("--prompt");
        psi.ArgumentList.Add(prompt);

        var process = Process.Start(psi);
        if (process is null)
        {
            throw new InvalidOperationException("Failed to start codex.");
        }
    }
}
