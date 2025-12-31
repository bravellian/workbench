namespace Workbench.Core;

/// <summary>
/// Payload describing doc summary results.
/// </summary>
/// <param name="FilesUpdated">Number of files updated.</param>
/// <param name="NotesAdded">Number of change-note entries added.</param>
/// <param name="UpdatedFiles">Files updated with notes.</param>
/// <param name="SkippedFiles">Files skipped due to filtering or validation.</param>
/// <param name="Errors">Errors encountered during summarization.</param>
/// <param name="Warnings">Warnings encountered during summarization.</param>
public sealed record DocSummaryData(
    [property: JsonPropertyName("filesUpdated")] int FilesUpdated,
    [property: JsonPropertyName("notesAdded")] int NotesAdded,
    [property: JsonPropertyName("updatedFiles")] IList<string> UpdatedFiles,
    [property: JsonPropertyName("skippedFiles")] IList<string> SkippedFiles,
    [property: JsonPropertyName("errors")] IList<string> Errors,
    [property: JsonPropertyName("warnings")] IList<string> Warnings);
