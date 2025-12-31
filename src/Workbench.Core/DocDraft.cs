namespace Workbench.Core;

/// <summary>
/// AI-generated draft content for a document.
/// </summary>
/// <param name="Title">Suggested title.</param>
/// <param name="Body">Draft body markdown.</param>
public sealed record DocDraft(
    string Title,
    string Body);
