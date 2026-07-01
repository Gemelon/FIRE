using System.Globalization;
using System.Resources;

namespace FIRE.Localization;

internal static class ApiLocalizer
{
    private static readonly ResourceManager ResourceManager = new("FIRE.Resources.ApiStrings", typeof(ApiLocalizer).Assembly);

    /// <summary>
    /// Resolves a localized resource string by key.
    /// </summary>
    /// <param name="key">Resource key to look up.</param>
    /// <param name="culture">Optional target culture. Uses <see cref="CultureInfo.CurrentUICulture"/> when omitted.</param>
    /// <returns>
    /// The localized value for the requested key. Falls back to <c>en-US</c> and finally to the key itself.
    /// </returns>
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

    /// <summary>
    /// Resolves and formats a localized resource string.
    /// </summary>
    /// <param name="key">Resource key to look up.</param>
    /// <param name="culture">Optional target culture. Uses <see cref="CultureInfo.CurrentUICulture"/> when omitted.</param>
    /// <param name="args">Format arguments applied to the localized format string.</param>
    /// <returns>The formatted localized string.</returns>
    public static string Format(string key, CultureInfo? culture = null, params object[] args)
    {
        return string.Format(culture ?? CultureInfo.CurrentUICulture, Get(key, culture), args);
    }
}
