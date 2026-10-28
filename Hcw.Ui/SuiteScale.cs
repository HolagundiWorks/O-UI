using Microsoft.UI.Xaml;

namespace Hcw.Ui;

/// <summary>
/// Reads the suite scale canon from <c>SuiteStyles.xaml</c>. Use these helpers instead of
/// hard-coded 32 / 8 / 16 values so OSAS, O-Studio, O-CoreOp and O-Draft stay in lockstep.
/// </summary>
public static class SuiteScale
{
    public static double ControlMinHeight => Double("SuiteControlMinHeight", 36);
    public static double NavItemMinHeight => Double("SuiteNavItemMinHeight", 40);
    public static double StackGap => Double("SuiteStackGap", 8);
    public static double FieldGap => Double("SuiteFieldGap", 12);
    public static double SectionGap => Double("SuiteSectionGap", 20);
    public static double CardGap => Double("SuiteCardGap", 16);
    public static double RailWidth => Double("SuiteRailWidth", 56);
    public static double PaneWidth => Double("SuitePaneWidth", 260);
    public static double FontBody => Double("SuiteFontBody", 13);
    public static double FontSection => Double("SuiteFontSection", 15);
    public static double FontTitle => Double("SuiteFontTitle", 22);

    public static Thickness PageMargin => Thickness("PageMargin", new Thickness(16, 12, 16, 16));
    public static Thickness ContentPadding => Thickness("SuiteContentPadding", new Thickness(12, 10, 12, 12));
    public static Thickness ControlPadding => Thickness("SuiteControlPadding", new Thickness(12, 4, 12, 4));
    public static Thickness StageMargin => Thickness("SuiteStageMargin", new Thickness(8));
    public static Thickness CardPadding => Thickness("SuiteCardPadding", new Thickness(12, 10, 12, 10));
    public static Thickness DialogPadding => Thickness("SuiteDialogPadding", new Thickness(16, 12, 16, 12));

    public static CornerRadius CornerButton => Corner("SuiteCornerButton", new CornerRadius(2));
    public static CornerRadius CornerControl => Corner("SuiteCornerControl", new CornerRadius(2));
    public static CornerRadius CornerChip => Corner("SuiteCornerChip", new CornerRadius(2));
    public static CornerRadius CornerCard => Corner("SuiteCornerCard", new CornerRadius(2));

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
