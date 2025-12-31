using Microsoft.VisualStudio.TestTools.UnitTesting;
using Workbench;
using Workbench.Core;
using Workbench.Core.Voice;

namespace Workbench.Tests;

[TestClass]
public sealed class VoiceTests
{
    [TestMethod]
    public void AudioLimiterHonorsMaxDuration()
    {
        var format = new AudioFormat(16000, 1, 16);
        var limits = AudioLimiter.Calculate(format, TimeSpan.FromMinutes(4), maxBytes: 50 * 1024 * 1024);

        Assert.AreEqual(TimeSpan.FromMinutes(4), limits.MaxDuration);
        Assert.AreEqual(16000 * 240, limits.MaxFrames);
    }

    [TestMethod]
    public void AudioLimiterHonorsMaxBytes()
    {
        var format = new AudioFormat(16000, 1, 16);
        var limits = AudioLimiter.Calculate(format, TimeSpan.FromMinutes(10), maxBytes: 1 * 1024 * 1024);
        var expectedSeconds = (1 * 1024 * 1024) / (double)format.BytesPerSecond;

        Assert.AreEqual(expectedSeconds, limits.MaxDuration.TotalSeconds, 0.01);
        Assert.IsTrue(limits.MaxDuration < TimeSpan.FromMinutes(10));
    }

    [TestMethod]
    public void TranscriptCombinerAddsChunkMarkers()
    {
        var combined = TranscriptCombiner.Combine(new[] { "first", "second" });

        var expected = "[chunk 1]\nfirst\n\n---\n\n[chunk 2]\nsecond";
        Assert.AreEqual(expected, combined);
    }

    [TestMethod]
    public void DocFrontMatterIncludesVoiceSource()
    {
        var source = new DocSourceInfo(
            "voice",
            "Excerpt",
            new DocAudioInfo(16000, 1, "wav"));

        var repoRoot = Path.Combine(Path.GetTempPath(), "workbench-tests", Guid.NewGuid().ToString("N"));
        var created = DocService.CreateGeneratedDoc(
            repoRoot,
            WorkbenchConfig.Default,
            "spec",
            "Voice doc",
            "# Voice doc\n",
            path: null,
            workItems: new List<string>(),
            codeRefs: new List<string>(),
            tags: new List<string>(),
            related: new List<string>(),
            status: "draft",
            source: source,
            force: false);

        var serialized = File.ReadAllText(created.Path);
        StringAssert.Contains(serialized, "title: Voice doc", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "type: spec", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "source:", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "kind: voice", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "sample_rate_hz: 16000", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "format: wav", StringComparison.Ordinal);
        StringAssert.Contains(serialized, "path: /docs/10-product/voice-doc.md", StringComparison.Ordinal);

        File.Delete(created.Path);
    }

    [TestMethod]
    public void DocPathResolverUsesDocTypeDefaults()
    {
        var repoRoot = Path.Combine(Path.GetTempPath(), "workbench-tests");
        var config = WorkbenchConfig.Default;
        var title = "Voice Doc";
        var slug = WorkItemService.Slugify(title);

        var created = DocService.CreateGeneratedDoc(
            repoRoot,
            config,
            "spec",
            title,
            "# Voice Doc\n",
            path: null,
            workItems: new List<string>(),
            codeRefs: new List<string>(),
            tags: new List<string>(),
            related: new List<string>(),
            status: "draft",
            source: null,
            force: false);

        var expected = Path.Combine(repoRoot, "docs", "10-product", $"{slug}.md");
        Assert.AreEqual(expected, created.Path);

        File.Delete(created.Path);
    }
}
