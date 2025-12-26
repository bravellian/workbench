namespace Workbench;

public static class PullRequestBuilder
{
    public static string BuildBody(WorkItem item)
    {
        var summary = ExtractSection(item.Body, "Summary");
        var criteria = ExtractSection(item.Body, "Acceptance criteria");
        var lines = new List<string>();

        if (!string.IsNullOrWhiteSpace(summary))
        {
            lines.Add("## Summary");
            lines.Add(summary.Trim());
            lines.Add(string.Empty);
        }

        if (!string.IsNullOrWhiteSpace(criteria))
        {
            lines.Add("## Acceptance criteria");
            lines.Add(criteria.Trim());
            lines.Add(string.Empty);
        }

        var related = BuildRelated(item);
        if (related.Count > 0)
        {
            lines.Add("## Related");
            lines.AddRange(related);
            lines.Add(string.Empty);
        }

        return string.Join("\n", lines).TrimEnd();
    }

    private static string ExtractSection(string body, string heading)
    {
        var lines = body.Replace("\r\n", "\n").Split('\n');
        var start = -1;
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim().Equals($"## {heading}", StringComparison.OrdinalIgnoreCase))
            {
                start = i + 1;
                break;
            }
        }
        if (start == -1)
        {
            return string.Empty;
        }

        var collected = new List<string>();
        for (var i = start; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.TrimStart().StartsWith("## ", StringComparison.Ordinal))
            {
                break;
            }
            collected.Add(line);
        }
        return string.Join("\n", collected).Trim();
    }

    private static List<string> BuildRelated(WorkItem item)
    {
        var list = new List<string>();
        void AddLinks(IEnumerable<string> links, string label)
        {
            var entries = links.Where(link => !string.IsNullOrWhiteSpace(link)).ToList();
            if (entries.Count == 0)
            {
                return;
            }
            list.Add($"- {label}:");
            foreach (var entry in entries)
            {
                list.Add($"  - {entry}");
            }
        }

        AddLinks(item.Related.Specs, "Specs");
        AddLinks(item.Related.Adrs, "ADRs");
        AddLinks(item.Related.Files, "Files");
        AddLinks(item.Related.Prs, "PRs");
        AddLinks(item.Related.Issues, "Issues");
        return list;
    }
}
