namespace Workbench;

public sealed record RelatedLinks(
    List<string> Specs,
    List<string> Adrs,
    List<string> Files,
    List<string> Prs,
    List<string> Issues);

public sealed record WorkItem(
    string Id,
    string Type,
    string Status,
    string Title,
    string? Priority,
    string? Owner,
    string Created,
    string? Updated,
    List<string> Tags,
    RelatedLinks Related,
    string Slug,
    string Path,
    string Body);
