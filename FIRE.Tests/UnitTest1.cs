using System.Globalization;

namespace FIRE.Tests;

public class UnitTest1
{
    [Fact]
    public void ApplyStringReplacement_ExactPattern_ReplacesOnlyMatchingSubstring()
    {
        var input = "MP4 Base Media v1 [IS0 14496-12:2003]";
        var result = FIRECatalog.ApplyStringReplacement(input, "IS0 14496", "DJI");

        Assert.Equal("MP4 Base Media v1 [DJI-12:2003]", result);
    }

    [Fact]
    public void ApplyStringReplacement_WildcardPattern_ReplacesMatchedSegment()
    {
        var input = "MP4 Base Media v1 [IS0 14496-12:2003]";
        var result = FIRECatalog.ApplyStringReplacement(input, "*IS0 14496*", "DJI");

        Assert.Equal("DJI", result);
    }

    [Fact]
    public void ApplyStringReplacement_RegexPrefix_UsesRegexReplacement()
    {
        var input = "SM-S938B and SM-F766B";
        var result = FIRECatalog.ApplyStringReplacement(input, "regex:SM-(S938B|F766B)", "Samsung");

        Assert.Equal("Samsung and Samsung", result);
    }

    [Fact]
    public void ResolveKeywordDefaultValue_DatetimeNow_UsesProvidedNowValueAndNormalizes()
    {
        var keywordConfig = new AvailableKeywordConfiguration
        {
            DataType = "DATETIME",
            Default = "NOW"
        };

        var fixedNow = new DateTime(2026, 12, 31, 23, 59, 58, DateTimeKind.Local);
        var result = FIRECatalog.ResolveKeywordDefaultValue(keywordConfig, fixedNow);

        Assert.Equal("2026:12:31 23:59:58", result);
    }

    [Fact]
    public void ResolveKeywordDefaultValue_DatetimeString_NormalizesToDatabaseFormat()
    {
        var keywordConfig = new AvailableKeywordConfiguration
        {
            DataType = "DATETIME",
            Default = "2024-12-31 00:00:00"
        };

        var result = FIRECatalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

        Assert.Equal("2024:12:31 00:00:00", result);
    }

    [Fact]
    public void ResolveKeywordDefaultValue_InvalidDatetimeDefault_ReturnsNA()
    {
        var keywordConfig = new AvailableKeywordConfiguration
        {
            DataType = "DATETIME",
            Default = "not-a-date"
        };

        var result = FIRECatalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

        Assert.Equal("NA", result);
    }

    [Fact]
    public void ResolveKeywordDefaultValue_StringDefault_ReturnsDefaultUnchanged()
    {
        var keywordConfig = new AvailableKeywordConfiguration
        {
            DataType = "STRING",
            Default = "UnknownCamera"
        };

        var result = FIRECatalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

        Assert.Equal("UnknownCamera", result);
    }
}
