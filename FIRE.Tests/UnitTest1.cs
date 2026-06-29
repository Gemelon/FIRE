using System;
using System.Globalization;
using System.IO;
using System.Reflection;

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
        using var catalog = CreateCatalog();
        var result = catalog.ResolveKeywordDefaultValue(keywordConfig, fixedNow);

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

        using var catalog = CreateCatalog();
        var result = catalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

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

        using var catalog = CreateCatalog();
        var result = catalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

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

        using var catalog = CreateCatalog();
        var result = catalog.ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);

        Assert.Equal("UnknownCamera", result);
    }

    [Fact]
    public void ParseTemplate_DatetimeKeywordSupport_ResolvesNamedAndFormatSuffixes()
    {
        using var catalog = CreateCatalog();
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["CapturedAt"] = "2026:07:02 14:05:09"
        };
        var metadataTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["CapturedAt"] = "DATETIME"
        };

        var result = InvokeParseTemplate(catalog, "{CapturedAt.Year}_{CapturedAt.Minute}_{CapturedAt:yyyy-MM-dd}", metadata, metadataTypes);

        Assert.Equal("2026_05_2026-07-02", result);
    }

    [Fact]
    public void ParseTemplate_NonDatetimeKeywordWithSuffix_DoesNotApplyDateFallback()
    {
        using var catalog = CreateCatalog();
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Model"] = "Canon"
        };
        var metadataTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Model"] = "STRING"
        };

        var result = InvokeParseTemplate(catalog, "{Model.Year}", metadata, metadataTypes);

        Assert.Equal("Canon", result);
    }

    private static string InvokeParseTemplate(FIRECatalog catalog, string template, Dictionary<string, string> metadata, Dictionary<string, string> metadataTypes)
    {
        var parseTemplate = typeof(FIRECatalog).GetMethod("ParseTemplate", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(parseTemplate);

        return (string)parseTemplate!.Invoke(catalog, [template, metadata, @"C:\\Temp\\source.jpg", null, metadataTypes])!;
    }

    private static FIRECatalog CreateCatalog()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), $"fire-tests-{Guid.NewGuid():N}.db");
        var configuration = new FIREConfigration();
        var database = new FIREDatabase(databasePath, recreateIfExists: true);
        return new FIRECatalog(configuration, database);
    }
}
