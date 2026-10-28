using System.Text.Json;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Hcw.Ui;

/// <summary>
/// Shared light/dark preference for the suite
/// (<c>%LOCALAPPDATA%\OSAS\appearance.json</c>), kept in sync with O-Draft's
/// <c>SuiteAppearancePrefs</c> contract.
/// </summary>
public static class SuiteAppearance
{
    static ElementTheme _current = Load();

    public static event Action<ElementTheme>? Changed;

    public static ElementTheme Current => _current;

    public static bool IsDark => _current == ElementTheme.Dark;

    public static void Apply(Window window)
    {
        if (window.Content is FrameworkElement root)
            root.RequestedTheme = _current;
    }

    public static void Set(ElementTheme theme)
    {
        if (theme is not (ElementTheme.Light or ElementTheme.Dark))
            theme = ElementTheme.Light;
        if (_current == theme)
            return;
        _current = theme;
        Save(theme);
        Changed?.Invoke(theme);
    }

    public static void Toggle() =>
        Set(IsDark ? ElementTheme.Light : ElementTheme.Dark);

    public static Brush? TryBrush(string key)
    {
        try
        {
            if (Application.Current?.Resources is null)
                return null;
            if (Application.Current.Resources.TryGetValue(key, out var value) && value is Brush brush)
                return brush;
        }
        catch
        {
        }
        return null;
    }

    static ElementTheme Load()
    {
        try
        {
            var path = PrefPath();
            if (!File.Exists(path))
                return ElementTheme.Dark;
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (doc.RootElement.TryGetProperty("theme", out var theme) &&
                theme.GetString() is { } name)
            {
                if (name.Equals("light", StringComparison.OrdinalIgnoreCase))
                    return ElementTheme.Light;
                if (name.Equals("dark", StringComparison.OrdinalIgnoreCase))
                    return ElementTheme.Dark;
            }
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
        }
        return ElementTheme.Dark;
    }

    static void Save(ElementTheme theme)
    {
        try
        {
            var path = PrefPath();
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(
                new { theme = theme == ElementTheme.Dark ? "dark" : "light" },
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
        }
    }

    static string PrefPath() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "OSAS",
            "appearance.json");
}
