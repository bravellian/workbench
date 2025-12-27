using System.Text.Json.Serialization;

namespace Workbench;

public sealed record DoctorOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DoctorData Data);

public sealed record DoctorData(
    [property: JsonPropertyName("repoRoot")] string RepoRoot,
    [property: JsonPropertyName("checks")] List<DoctorCheck> Checks);

public sealed record DoctorCheck(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("details")] DoctorCheckDetails? Details);

public sealed record DoctorCheckDetails(
    [property: JsonPropertyName("version")] string? Version,
    [property: JsonPropertyName("error")] string? Error,
    [property: JsonPropertyName("reason")] string? Reason,
    [property: JsonPropertyName("path")] string? Path,
    [property: JsonPropertyName("missing")] List<string>? Missing,
    [property: JsonPropertyName("schemaErrors")] List<string>? SchemaErrors);

public sealed record ScaffoldOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ScaffoldData Data);

public sealed record ScaffoldData(
    [property: JsonPropertyName("created")] List<string> Created,
    [property: JsonPropertyName("skipped")] List<string> Skipped,
    [property: JsonPropertyName("configPath")] string ConfigPath);

public sealed record ConfigOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ConfigData Data);

public sealed record ConfigData(
    [property: JsonPropertyName("config")] WorkbenchConfig Config,
    [property: JsonPropertyName("sources")] ConfigSources Sources);

public sealed record ConfigSources(
    [property: JsonPropertyName("defaults")] bool Defaults,
    [property: JsonPropertyName("repoConfig")] string RepoConfig);

public sealed record ItemCreateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemCreateData Data);

public sealed record ItemCreateData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("path")] string Path);

public sealed record ItemListOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemListData Data);

public sealed record ItemListData(
    [property: JsonPropertyName("items")] List<ItemSummary> Items);

public sealed record ItemSummary(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("path")] string Path);

public sealed record ItemShowOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemShowData Data);

public sealed record ItemShowData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);

public sealed record ItemStatusOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemStatusData Data);

public sealed record ItemStatusData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);

public sealed record ItemCloseOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemCloseData Data);

public sealed record ItemCloseData(
    [property: JsonPropertyName("item")] WorkItemPayload Item,
    [property: JsonPropertyName("moved")] bool Moved);

public sealed record ItemMoveOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemMoveData Data);

public sealed record ItemMoveData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);

public sealed record ItemRenameOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ItemRenameData Data);

public sealed record ItemRenameData(
    [property: JsonPropertyName("item")] WorkItemPayload Item);

public sealed record BoardOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] BoardData Data);

public sealed record BoardData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("counts")] Dictionary<string, int> Counts);

public sealed record PromoteOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] PromoteData Data);

public sealed record PromoteData(
    [property: JsonPropertyName("item")] WorkItemPayload Item,
    [property: JsonPropertyName("branch")] string Branch,
    [property: JsonPropertyName("commit")] CommitInfo Commit,
    [property: JsonPropertyName("pushed")] bool Pushed,
    [property: JsonPropertyName("pr")] string? Pr);

public sealed record CommitInfo(
    [property: JsonPropertyName("sha")] string Sha,
    [property: JsonPropertyName("message")] string Message);

public sealed record PrOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] PrData Data);

public sealed record PrData(
    [property: JsonPropertyName("pr")] string Pr,
    [property: JsonPropertyName("item")] string Item);

public sealed record ValidateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] ValidateData Data);

public sealed record ValidateData(
    [property: JsonPropertyName("errors")] List<string> Errors,
    [property: JsonPropertyName("warnings")] List<string> Warnings,
    [property: JsonPropertyName("counts")] ValidateCounts Counts);

public sealed record ValidateCounts(
    [property: JsonPropertyName("errors")] int Errors,
    [property: JsonPropertyName("warnings")] int Warnings,
    [property: JsonPropertyName("workItems")] int WorkItems,
    [property: JsonPropertyName("markdownFiles")] int MarkdownFiles);

public sealed record DocCreateOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocCreateData Data);

public sealed record DocCreateData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("workItems")] List<string> WorkItems);

public sealed record DocSyncOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocSyncData Data);

public sealed record DocSyncData(
    [property: JsonPropertyName("docsUpdated")] int DocsUpdated,
    [property: JsonPropertyName("itemsUpdated")] int ItemsUpdated,
    [property: JsonPropertyName("missingDocs")] List<string> MissingDocs,
    [property: JsonPropertyName("missingItems")] List<string> MissingItems);

public sealed record WorkItemPayload(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("priority")] string? Priority,
    [property: JsonPropertyName("owner")] string? Owner,
    [property: JsonPropertyName("created")] string Created,
    [property: JsonPropertyName("updated")] string? Updated,
    [property: JsonPropertyName("tags")] List<string> Tags,
    [property: JsonPropertyName("related")] RelatedLinksPayload Related,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("body")] string? Body);

public sealed record RelatedLinksPayload(
    [property: JsonPropertyName("specs")] List<string> Specs,
    [property: JsonPropertyName("adrs")] List<string> Adrs,
    [property: JsonPropertyName("files")] List<string> Files,
    [property: JsonPropertyName("prs")] List<string> Prs,
    [property: JsonPropertyName("issues")] List<string> Issues);
