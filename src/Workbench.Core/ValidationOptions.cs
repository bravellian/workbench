namespace Workbench.Core;

/// <summary>
/// Per-run validation options supplied by the CLI.
/// </summary>
/// <param name="LinkInclude">Repo-relative prefixes to include for link validation.</param>
/// <param name="LinkExclude">Repo-relative prefixes to exclude for link validation.</param>
/// <param name="SkipDocSchema">When true, skips doc front matter schema validation.</param>
public sealed record ValidationOptions(
    IList<string>? LinkInclude = null,
    IList<string>? LinkExclude = null,
    bool SkipDocSchema = false);
