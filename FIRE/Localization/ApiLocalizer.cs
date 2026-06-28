using System.Globalization;
using System.Resources;

namespace FIRE.Localization;

internal static class ApiLocalizer
{
    private static readonly ResourceManager ResourceManager = new("FIRE.Resources.ApiStrings", typeof(ApiLocalizer).Assembly);

    public static string Get(string key, CultureInfo? culture = null)
    {
        var effectiveCulture = culture ?? CultureInfo.CurrentUICulture;
        var value = ResourceManager.GetString(key, effectiveCulture);

        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        value = ResourceManager.GetString(key, CultureInfo.GetCultureInfo("en-US"));
        return string.IsNullOrWhiteSpace(value) ? key : value;
    }

    public static string Format(string key, CultureInfo? culture = null, params object[] args)
    {
        return string.Format(culture ?? CultureInfo.CurrentUICulture, Get(key, culture), args);
    }
}
