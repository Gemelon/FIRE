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

    [Fact]
    public void Parse_MetadataRules_GlobalAndExtension_AreDeserialized()
    {
        const string yaml = """
ConfigurationVersion: 1.30
MetadataRules:
  - When:
      Model: "Lavf60*"
    Set:
      Make: "Samsung"
      Model: "App"
FileExtensions:
  .mp4:
    AvailableKeyWords: {}
    MetadataRules:
      - When:
          Model: "Lavf61*"
        Set:
          Make: "Insta360"
          Model: "App"
""";

        var config = FIREConfigration.Parse(yaml);

        Assert.Single(config.MetadataRules);
        Assert.Single(config.FileExtensions[".mp4"].MetadataRules);
        Assert.Equal("Samsung", config.MetadataRules[0].Set["Make"]);
        Assert.Equal("Insta360", config.FileExtensions[".mp4"].MetadataRules[0].Set["Make"]);
    }

    [Theory]
    [InlineData("Lavf60.3", "Lavf60*", true)]
    [InlineData("LAVF61.1", "lavf61*", true)]
    [InlineData("Insta360", "regex:Insta[0-9]+", true)]
    [InlineData("Insta360", "regex:[", false)]
    [InlineData("DJI", "Lavf60*", false)]
    public void MetadataRulePatternMatches_SupportsLiteralWildcardAndRegex(string value, string pattern, bool expected)
    {
        var actual = FIRECatalog.MetadataRulePatternMatches(value, pattern);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ApplyMetadataRuleSet_WhenMatches_OverridesAndCreatesMetadata()
    {
        var record = new FIREDbRecord
        {
            FileMetaDatas =
            [
                new FIREFileMetaData { Key = "Model", Value = "Lavf60.2", DataSource = "EXIFTOOL", TypeName = "STRING" }
            ]
        };

        var rules = new List<MetadataRuleConfiguration>
        {
            new()
            {
                When = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Model"] = "Lavf60*"
                },
                Set = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Make"] = "Samsung",
                    ["Model"] = "App"
                }
            }
        };

        FIRECatalog.ApplyMetadataRuleSet(record, rules);

        var make = record.FileMetaDatas.Single(m => string.Equals(m.Key, "Make", StringComparison.OrdinalIgnoreCase));
        var model = record.FileMetaDatas.Single(m => string.Equals(m.Key, "Model", StringComparison.OrdinalIgnoreCase));

        Assert.Equal("Samsung", make.Value);
        Assert.Equal("RULE", make.DataSource);
        Assert.Equal("App", model.Value);
        Assert.Equal("RULE", model.DataSource);
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
