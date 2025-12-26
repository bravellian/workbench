using System.Text.Json;

namespace Workbench;

public sealed record WorkbenchConfig(
    PathsConfig Paths,
    IdsConfig Ids,
    GitConfig Git,
    GithubConfig Github)
{
    public static WorkbenchConfig Default => new(
        new PathsConfig(),
        new IdsConfig(),
        new GitConfig(),
        new GithubConfig());

    public static WorkbenchConfig Load(string repoRoot, out string? error)
    {
        error = null;
        var configPath = GetConfigPath(repoRoot);
        if (!File.Exists(configPath))
        {
            return Default;
        }

        try
        {
            var json = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<WorkbenchConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (config is null)
            {
                error = "Failed to parse config.";
                return Default;
            }
            return config;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return Default;
        }
    }

    public static string GetConfigPath(string repoRoot)
    {
        return Path.Combine(repoRoot, ".workbench", "config.json");
    }

    public string GetPrefix(string type)
    {
        return type switch
        {
            "bug" => Ids.Prefixes.Bug,
            "task" => Ids.Prefixes.Task,
            "spike" => Ids.Prefixes.Spike,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}

public sealed record PathsConfig
{
    public string DocsRoot { get; init; } = "docs";
    public string WorkRoot { get; init; } = "work";
    public string ItemsDir { get; init; } = "work/items";
    public string DoneDir { get; init; } = "work/done";
    public string TemplatesDir { get; init; } = "work/templates";
    public string WorkboardFile { get; init; } = "work/WORKBOARD.md";
}

public sealed record IdsConfig
{
    public int Width { get; init; } = 4;
    public PrefixesConfig Prefixes { get; init; } = new();
}

public sealed record PrefixesConfig
{
    public string Bug { get; init; } = "BUG";
    public string Task { get; init; } = "TASK";
    public string Spike { get; init; } = "SPIKE";
}

public sealed record GitConfig
{
    public string BranchPattern { get; init; } = "work/{id}-{slug}";
    public string CommitMessagePattern { get; init; } = "Promote {id}: {title}";
    public string DefaultBaseBranch { get; init; } = "main";
    public bool RequireCleanWorkingTree { get; init; } = true;
}

public sealed record GithubConfig
{
    public string Provider { get; init; } = "gh";
    public bool DefaultDraft { get; init; } = false;
}
