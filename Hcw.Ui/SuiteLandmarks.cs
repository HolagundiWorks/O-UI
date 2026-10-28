using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;

namespace Hcw.Ui;

/// <summary>
/// Draft-parity landmark helpers for suite shells (Navigation / Main / status).
/// Call from each app after the chrome tree is built. Full SuiteShell chrome types
/// (rail, stage host) still live outside this package; use these helpers on the
/// regions the app owns until SuiteShell ships here with landmarks built in.
/// </summary>
public static class SuiteLandmarks
{
    public static void Navigation(UIElement element, string? name = null)
    {
        AutomationProperties.SetLandmarkType(element, AutomationLandmarkType.Navigation);
        if (!string.IsNullOrWhiteSpace(name))
            AutomationProperties.SetName(element, name);
    }

    public static void Main(UIElement element, string? name = null)
    {
        AutomationProperties.SetLandmarkType(element, AutomationLandmarkType.Main);
        if (!string.IsNullOrWhiteSpace(name))
            AutomationProperties.SetName(element, name);
    }

    public static void Status(UIElement element, string? name = "Status")
    {
        AutomationProperties.SetLocalizedLandmarkType(element, "status");
        if (!string.IsNullOrWhiteSpace(name))
            AutomationProperties.SetName(element, name);
    }

    /// <summary>
    /// Apply the usual suite chrome trio: left rail / menu = Navigation,
    /// work stage = Main, footer / status strip = status.
    /// </summary>
    public static void ApplyShell(UIElement? navigation, UIElement? main, UIElement? status,
        string? navigationName = null, string? mainName = null, string? statusName = "Status")
    {
        if (navigation is not null)
            Navigation(navigation, navigationName);
        if (main is not null)
            Main(main, mainName);
        if (status is not null)
            Status(status, statusName);
    }
}
