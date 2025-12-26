using System.Diagnostics;

namespace Workbench;

public static class GithubService
{
    public sealed record CommandResult(int ExitCode, string StdOut, string StdErr);

    public static CommandResult Run(string repoRoot, params string[] args)
    {
        var psi = new ProcessStartInfo("gh")
        {
            WorkingDirectory = repoRoot,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (var arg in args)
        {
            psi.ArgumentList.Add(arg);
        }

        using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start gh.");
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        return new CommandResult(process.ExitCode, stdout.Trim(), stderr.Trim());
    }

    public static string CreatePullRequest(string repoRoot, string title, string body, string? baseBranch, bool draft)
    {
        var args = new List<string> { "pr", "create", "--title", title, "--body", body };
        if (!string.IsNullOrWhiteSpace(baseBranch))
        {
            args.Add("--base");
            args.Add(baseBranch);
        }
        if (draft)
        {
            args.Add("--draft");
        }

        var result = Run(repoRoot, args.ToArray());
        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException(result.StdErr.Length > 0 ? result.StdErr : "gh pr create failed.");
        }
        return result.StdOut.Trim();
    }
}
