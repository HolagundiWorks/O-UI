using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace O.Ui;

/// <summary>FontIcon sized for the activity rail or compact nav/tool chrome.</summary>
public sealed class SuiteIcon : FontIcon
{
    public enum SizeKind
    {
        Rail,
        Nav,
    }

    public SuiteIcon()
    {
        FontFamily = new FontFamily("Segoe Fluent Icons, Segoe MDL2 Assets");
        FontSize = 16;
        VerticalAlignment = VerticalAlignment.Center;
        HorizontalAlignment = HorizontalAlignment.Center;
    }

    public SuiteIcon(string glyph, SizeKind size) : this()
    {
        Glyph = glyph;
        FontSize = size == SizeKind.Rail ? 18 : 16;
    }
}
