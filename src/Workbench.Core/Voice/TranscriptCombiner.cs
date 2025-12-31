namespace Workbench.Core.Voice;

public static class TranscriptCombiner
{
    public static string Combine(IReadOnlyList<string> transcripts)
    {
        if (transcripts.Count == 0)
        {
            return string.Empty;
        }
        if (transcripts.Count == 1)
        {
            return transcripts[0].Trim();
        }

        var parts = new List<string>(transcripts.Count);
        for (var i = 0; i < transcripts.Count; i++)
        {
            var chunk = transcripts[i].Trim();
            parts.Add($"[chunk {i + 1}]\n{chunk}");
        }

        return string.Join("\n\n---\n\n", parts);
    }
}
