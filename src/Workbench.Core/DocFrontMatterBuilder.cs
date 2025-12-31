// Constructs front matter payloads for generated docs.
// Invariants: timestamps use ISO-8601 UTC; path is repo-relative with forward slashes.
namespace Workbench.Core;

public static class DocFrontMatterBuilder
{
    public static FrontMatter BuildGeneratedDocFrontMatter(
        string repoRoot,
        string docPath,
        string docType,
        string title,
        string body,
        IList<string> workItems,
        IList<string> codeRefs,
        IList<string> tags,
        IList<string> related,
        string? status,
        DocSourceInfo? source,
        DateTimeOffset now)
    {
        var relative = "/" + Path.GetRelativePath(repoRoot, docPath).Replace('\\', '/');
        var data = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["title"] = title,
            ["type"] = docType,
            ["created_utc"] = now.ToString("o", CultureInfo.InvariantCulture),
            ["updated_utc"] = now.ToString("o", CultureInfo.InvariantCulture),
            ["tags"] = tags.Cast<object?>().ToList(),
            ["related"] = related.Cast<object?>().ToList(),
            ["workbench"] = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["type"] = docType,
                ["workItems"] = workItems.Cast<object?>().ToList(),
                ["codeRefs"] = codeRefs.Cast<object?>().ToList(),
                ["path"] = relative,
                ["pathHistory"] = new List<string>()
            }
        };

        if (!string.IsNullOrWhiteSpace(status))
        {
            data["status"] = status;
        }

        if (source is not null)
        {
            var sourceMap = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["kind"] = source.Kind,
                ["audio"] = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["sample_rate_hz"] = source.Audio.SampleRateHz,
                    ["channels"] = source.Audio.Channels,
                    ["format"] = source.Audio.Format
                }
            };

            if (!string.IsNullOrWhiteSpace(source.Transcript))
            {
                sourceMap["transcript"] = source.Transcript;
            }

            data["source"] = sourceMap;
        }

        return new FrontMatter(data, body);
    }

    public static string BuildTranscriptExcerpt(string transcript, int maxChars)
    {
        if (string.IsNullOrWhiteSpace(transcript) || maxChars <= 0)
        {
            return string.Empty;
        }

        var trimmed = transcript.Trim();
        if (trimmed.Length <= maxChars)
        {
            return trimmed;
        }

        return trimmed[..maxChars].TrimEnd() + "...";
    }
}
