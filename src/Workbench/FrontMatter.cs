using YamlDotNet.Serialization;

namespace Workbench;

public sealed class FrontMatter
{
    public Dictionary<string, object?> Data { get; }
    public string Body { get; }

    public FrontMatter(Dictionary<string, object?> data, string body)
    {
        Data = data;
        Body = body;
    }

    public static bool TryParse(string content, out FrontMatter? frontMatter, out string? error)
    {
        frontMatter = null;
        error = null;
        var lines = content.Replace("\r\n", "\n").Split('\n');
        if (lines.Length < 3 || lines[0].Trim() != "---")
        {
            error = "Missing front matter start delimiter.";
            return false;
        }

        var endIndex = Array.FindIndex(lines, 1, line => line.Trim() == "---");
        if (endIndex <= 1)
        {
            error = "Missing front matter end delimiter.";
            return false;
        }

        var yamlText = string.Join("\n", lines[1..endIndex]);
        var body = string.Join("\n", lines[(endIndex + 1)..]).TrimStart('\n');

        try
        {
            var deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
            var data = deserializer.Deserialize<Dictionary<string, object?>>(yamlText) ?? new();
            frontMatter = new FrontMatter(data, body);
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    public string Serialize()
    {
        var serializer = new SerializerBuilder().Build();
        var yaml = serializer.Serialize(Data).TrimEnd('\n');
        return $"---\n{yaml}\n---\n\n{Body}".TrimEnd() + "\n";
    }
}
