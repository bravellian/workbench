namespace Workbench.Core;

public sealed record ValidationConfig(
    IList<string> LinkExclude,
    IList<string> DocExclude)
{
    public ValidationConfig()
        : this(new List<string>(), new List<string>())
    {
    }
}
