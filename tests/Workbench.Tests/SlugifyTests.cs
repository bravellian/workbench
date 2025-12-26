using Workbench;

namespace Workbench.Tests;

public class SlugifyTests
{
    [Fact]
    public void Slugify_NormalizesTitle()
    {
        var slug = WorkItemService.Slugify("Add promotion workflow!");
        Assert.Equal("add-promotion-workflow", slug);
    }
}
