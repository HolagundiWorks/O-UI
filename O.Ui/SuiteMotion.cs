using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;

namespace O.Ui;

/// <summary>Honours the system “animation effects” preference for suite page transitions.</summary>
public static class SuiteMotion
{
    static bool _enabled = true;

    /// <summary>
    /// When Windows animations are off, remove the named transition resources so pages
    /// do not animate against the user's preference.
    /// </summary>
    public static void RespectSystemPreference(params string[] resourceKeys)
    {
        _enabled = AnimationsEnabled();
        if (_enabled || Application.Current?.Resources is null)
            return;
        foreach (var key in resourceKeys)
        {
            if (Application.Current.Resources.ContainsKey(key))
                Application.Current.Resources[key] = null;
        }
    }

    public static void ApplyToFrame(Frame frame)
    {
        if (_enabled)
            return;
        frame.ContentTransitions = null;
    }

    static bool AnimationsEnabled()
    {
        try
        {
            var settings = new UISettings();
            return settings.AnimationsEnabled;
        }
        catch
        {
            return true;
        }
    }
}
