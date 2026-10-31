namespace O.Ui;

/// <summary>Segoe Fluent Icons glyphs used by the shared suite rail and widgets.</summary>
public static class SuiteIcons
{
    public const string Apps = "\uE71D";
    public const string Home = "\uE80F";
    public const string Report = "\uE9F9";
    public const string Tools = "\uE90F";
    public const string Assistant = "\uE99A";
    public const string About = "\uE946";
    public const string Time = "\uE121";
    public const string Calculate = "\uE1D0";
}

/// <summary>Icon pixel sizes from the suite scale canon.</summary>
public static class SuiteIconSize
{
    public static double Rail => SuiteScale.Double("SuiteIconRail", 15);
    public static double Nav => SuiteScale.Double("SuiteIconNav", 15);
    public static double Inline => SuiteScale.Double("SuiteIconInline", 15);
    public static double Header => SuiteScale.Double("SuiteIconHeader", 15);
    public static double Stroke => SuiteScale.Double("SuiteIconStroke", 1.5);
    public static double Tile => SuiteScale.Double("SuiteToolTile", 36);
}
