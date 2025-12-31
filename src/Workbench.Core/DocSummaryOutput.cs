namespace Workbench.Core;

/// <summary>
/// JSON response envelope for doc summary output.
/// </summary>
/// <param name="Ok">True when summarization completed without errors.</param>
/// <param name="Data">Summary results and diagnostics.</param>
public sealed record DocSummaryOutput(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("data")] DocSummaryData Data);
