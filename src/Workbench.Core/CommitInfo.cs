namespace Workbench.Core;

/// <summary>
/// Commit metadata for promote output.
/// </summary>
/// <param name="Sha">Commit SHA.</param>
/// <param name="Message">Commit message.</param>
public sealed record CommitInfo(
    [property: JsonPropertyName("sha")] string Sha,
    [property: JsonPropertyName("message")] string Message);
