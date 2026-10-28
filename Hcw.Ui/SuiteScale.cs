namespace Hcw.Ui;

/// <summary>
/// Shared spacing for suite chrome (matches <c>Space8</c> / <c>Space12</c> / <c>Space20</c> in SuiteStyles).
/// Office apps use these gaps; O-Draft keeps its own CAD density locally.
/// </summary>
public static class SuiteScale
{
    public const double StackGap = 8;
    public const double FieldGap = 12;
    public const double SectionGap = 20;
}
