using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace O.Ui.Controls;

/// <summary>Lays buttons out in a row that wraps onto the next line when the page is narrow.</summary>
public sealed class FlexBar : Panel
{
    protected override Size MeasureOverride(Size availableSize)
    {
        var limit = double.IsInfinity(availableSize.Width) ? 1600 : Math.Max(0, availableSize.Width);
        double x = 0, y = 0, row = 0, width = 0;
        foreach (var child in Children)
        {
            child.Measure(new Size(limit, double.PositiveInfinity));
            var w = child.DesiredSize.Width;
            var h = child.DesiredSize.Height;
            if (x > 0 && x + w > limit)
            {
                y += row + 8;
                x = 0;
                row = 0;
            }
            if (x > 0)
                x += 8;
            x += w;
            row = Math.Max(row, h);
            width = Math.Max(width, x);
        }
        return new Size(Math.Min(limit, width), y + row);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        double x = 0, y = 0, row = 0;
        foreach (var child in Children)
        {
            var w = child.DesiredSize.Width;
            var h = child.DesiredSize.Height;
            if (x > 0 && x + w > finalSize.Width + 0.5)
            {
                y += row + 8;
                x = 0;
                row = 0;
            }
            child.Arrange(new Rect(x, y, w, h));
            x += w + 8;
            row = Math.Max(row, h);
        }
        return finalSize;
    }
}
