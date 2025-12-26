using Workbench;

namespace Workbench.Tests;

public class FrontMatterTests
{
    [Fact]
    public void ParseAndSerialize_RoundTripsBody()
    {
        var content = """
            ---
            id: TASK-0001
            type: task
            status: draft
            created: 2025-01-01
            related:
              specs: []
              adrs: []
              files: []
              prs: []
              issues: []
            ---

            # TASK-0001 - Sample

            ## Summary
            Hello
            """;

        var ok = FrontMatter.TryParse(content, out var frontMatter, out var error);
        Assert.True(ok, error);
        Assert.NotNull(frontMatter);
        Assert.Contains("TASK-0001", frontMatter!.Serialize());
        Assert.Contains("## Summary", frontMatter.Serialize());
    }
}
