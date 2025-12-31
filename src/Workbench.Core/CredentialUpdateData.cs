namespace Workbench.Core;

/// <summary>
/// Payload describing a credentials.env update.
/// </summary>
/// <param name="Path">Path to the credentials file.</param>
/// <param name="Key">Environment variable name.</param>
/// <param name="Created">True when a new entry was created.</param>
/// <param name="Updated">True when an existing entry was updated.</param>
/// <param name="Removed">True when an entry was removed.</param>
public sealed record CredentialUpdateData(
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("created")] bool Created,
    [property: JsonPropertyName("updated")] bool Updated,
    [property: JsonPropertyName("removed")] bool Removed);
