namespace Workbench.Core;

/// <summary>
/// Internal result model for env file updates.
/// </summary>
/// <param name="Path">Path to the env file.</param>
/// <param name="Key">Environment variable name.</param>
/// <param name="Created">True when a new entry was created.</param>
/// <param name="Updated">True when an existing entry was updated.</param>
/// <param name="Removed">True when an entry was removed.</param>
public sealed record EnvUpdateResult(
    string Path,
    string Key,
    bool Created,
    bool Updated,
    bool Removed);
