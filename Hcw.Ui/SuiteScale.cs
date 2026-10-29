using Microsoft.UI.Xaml;

namespace Hcw.Ui;

/// <summary>
/// Reads the suite scale canon (compact density, as polished in O-Draft) from <c>SuiteStyles.xaml</c>. Use these helpers instead of
/// hard-coded 32 / 8 / 16 values so OSAS, O-Studio, O-CoreOp and O-Draft stay in lockstep.
/// </summary>
public static class SuiteScale
{
    public static double ControlMinHeight => Double("SuiteControlMinHeight", 26);
    public static double NavItemMinHeight => Double("SuiteNavItemMinHeight", 28);
    public static double StackGap => Double("SuiteStackGap", 8);
    public static double FieldGap => Double("SuiteFieldGap", 8);
    public static double SectionGap => Double("SuiteSectionGap", 16);
    public static double CardGap => Double("SuiteCardGap", 12);
    public static double RailWidth => Double("SuiteRailWidth", 56);
    public static double PaneWidth => Double("SuitePaneWidth", 240);
    public static double FontBody => Double("SuiteFontBody", 12);
    public static double FontSection => Double("SuiteFontSection", 13);
    public static double FontTitle => Double("SuiteFontTitle", 18);

    public static double Space1 => Double("Space1", 4);
    public static double Space2 => Double("Space2", 8);
    public static double Space3 => Double("Space3", 12);
    public static double Space4 => Double("Space4", 16);
    public static double Space5 => Double("Space5", 20);
    public static double Space6 => Double("Space6", 24);
    public static double MenuBarHeight => Double("SuiteMenuBarHeight", 24);
    public static double MenuItemHeight => Double("SuiteMenuItemHeight", 22);
    public static double TabHeight => Double("SuiteTabHeight", 28);
    public static double StatusBarHeight => Double("SuiteStatusBarHeight", 22);
    public static double PanelHeaderHeight => Double("SuitePanelHeaderHeight", 24);
    public static double PropertyRowHeight => Double("SuitePropertyRowHeight", 26);
    public static double ControlHeightCompact => Double("SuiteControlHeightCompact", 22);
    public static double FontCaption => Double("SuiteFontCaption", 11);

    public static Thickness PageMargin => Thickness("PageMargin", new Thickness(16, 12, 16, 16));
    public static Thickness ContentPadding => Thickness("SuiteContentPadding", new Thickness(12, 8, 12, 8));
    public static Thickness ControlPadding => Thickness("SuiteControlPadding", new Thickness(8, 2, 8, 2));
    public static Thickness StageMargin => Thickness("SuiteStageMargin", new Thickness(8));
    public static Thickness CardPadding => Thickness("SuiteCardPadding", new Thickness(12, 8, 12, 8));
    public static Thickness DialogPadding => Thickness("SuiteDialogPadding", new Thickness(12));

    public static CornerRadius CornerButton => Corner("SuiteCornerButton", new CornerRadius(4));
    public static CornerRadius CornerControl => Corner("SuiteCornerControl", new CornerRadius(4));
    public static CornerRadius CornerChip => Corner("SuiteCornerChip", new CornerRadius(4));
    public static CornerRadius CornerCard => Corner("SuiteCornerCard", new CornerRadius(4));

    public static double Double(string key, double fallback = 0)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is double d)
            return d;
        return fallback;
    }

    public static Thickness Thickness(string key, Thickness fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Thickness t)
            return t;
        return fallback;
    }

    public static CornerRadius Corner(string key, CornerRadius fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is CornerRadius c)
            return c;
        return fallback;
    }
}
