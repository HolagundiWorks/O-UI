using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Hcw.Ui;

/// <summary>
/// Shared suite chrome host: activity rail beside a stage. Applies Draft-parity
/// Navigation / Main landmarks on the rail and stage.
/// </summary>
public sealed class SuiteShell : Grid
{
    readonly ContentPresenter _stageHost = new();

    public SuiteActivityRail Rail { get; } = new();

    public UIElement? Stage
    {
        get => _stageHost.Content as UIElement;
        set
        {
            _stageHost.Content = value;
            if (value is not null)
                SuiteLandmarks.Main(value, "Main");
        }
    }

    public SuiteShell()
    {
        ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        if (Application.Current?.Resources.TryGetValue("SidebarBackgroundBrush", out var sidebar) == true &&
            sidebar is Brush brush)
            Rail.Background = brush;
        else
            Rail.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);

        Children.Add(Rail);
        Grid.SetColumn(_stageHost, 1);
        _stageHost.Padding = new Thickness(20, 16, 20, 16);
        Children.Add(_stageHost);

        SuiteLandmarks.ApplyShell(Rail, null, null, navigationName: "Activity");
    }
}
