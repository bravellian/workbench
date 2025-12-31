// Builds default document bodies and enforces title headers.
// Invariants: title header is always the first line in markdown output.
namespace Workbench.Core;

public static class DocBodyBuilder
{
    public static string EnsureTitle(string body, string title)
    {
        var normalizedBody = body?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedBody))
        {
            return $"# {title}\n";
        }

        var lines = normalizedBody.Replace("\r\n", "\n").Split('\n').ToList();
        if (lines.Count > 0 && lines[0].TrimStart().StartsWith("# ", StringComparison.Ordinal))
        {
            lines[0] = $"# {title}";
            return string.Join("\n", lines).TrimEnd() + "\n";
        }

        return $"# {title}\n\n{normalizedBody}".TrimEnd() + "\n";
    }

    public static string BuildSkeleton(string docType, string title)
    {
        var header = $"# {title}\n\n";
        return docType.Trim().ToLowerInvariant() switch
        {
            "adr" => header + "## Status\n\n## Context\n\n## Decision\n\n## Consequences\n",
            "spec" => header + "## Summary\n\n## Goals\n\n## Non-goals\n\n## Requirements\n",
            "runbook" => header + "## Purpose\n\n## Steps\n\n## Rollback\n",
            _ => header + "## Notes\n"
        };
    }
}
