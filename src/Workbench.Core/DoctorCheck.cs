namespace Workbench.Core;

public sealed record DoctorCheck(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("details")] DoctorCheckDetails? Details);
