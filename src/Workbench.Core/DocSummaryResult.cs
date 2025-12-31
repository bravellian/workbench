namespace Workbench.Core;

/// <summary>
/// Internal result model for doc summarization operations.
/// </summary>
/// <param name="FilesUpdated">Number of files updated.</param>
/// <param name="NotesAdded">Number of change notes added.</param>
/// <param name="UpdatedFiles">List of updated files.</param>
/// <param name="SkippedFiles">List of skipped files.</param>
/// <param name="Errors">Errors encountered.</param>
/// <param name="Warnings">Warnings encountered.</param>
public sealed record DocSummaryResult(
    int FilesUpdated,
    int NotesAdded,
    IList<string> UpdatedFiles,
    IList<string> SkippedFiles,
    IList<string> Errors,
    IList<string> Warnings);
