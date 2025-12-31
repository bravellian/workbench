namespace Workbench.Core;

public static class DocTitleHelper
{
    public static string FromTranscript(string transcript, int maxWords = 8)
    {
        if (string.IsNullOrWhiteSpace(transcript))
        {
            return "Voice note";
        }

        var words = transcript
            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Take(maxWords)
            .ToList();

        if (words.Count == 0)
        {
            return "Voice note";
        }

        var title = string.Join(" ", words).Trim();
        return title.Length == 0 ? "Voice note" : title;
    }
}
