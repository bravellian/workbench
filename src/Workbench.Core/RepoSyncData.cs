namespace Workbench.Core;

/// <summary>
/// Payload describing repo-wide sync results.
/// </summary>
/// <param name="Items">Work item sync results when executed.</param>
/// <param name="Docs">Doc sync results when executed.</param>
/// <param name="Nav">Navigation sync results when executed.</param>
/// <param name="DryRun">True when no changes were applied.</param>
public sealed record RepoSyncData(
    [property: JsonPropertyName("items")] ItemSyncData? Items,
    [property: JsonPropertyName("docs")] DocSyncData? Docs,
    [property: JsonPropertyName("nav")] NavSyncData? Nav,
    [property: JsonPropertyName("dryRun")] bool DryRun);
