namespace Workbench.Core;

/// <summary>
/// Payload describing scaffold results.
/// </summary>
/// <param name="Created">Paths created during scaffolding.</param>
/// <param name="Skipped">Paths skipped because they already existed.</param>
/// <param name="ConfigPath">Path to the generated config file.</param>
public sealed record ScaffoldData(
    [property: JsonPropertyName("created")] IList<string> Created,
    [property: JsonPropertyName("skipped")] IList<string> Skipped,
    [property: JsonPropertyName("configPath")] string ConfigPath);
