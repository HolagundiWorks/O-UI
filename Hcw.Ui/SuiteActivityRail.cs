using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;

namespace Hcw.Ui;

/// <summary>
/// Left activity rail for the suite launcher: icon destinations plus an optional footer.
/// </summary>
public sealed class SuiteActivityRail : Grid
{
    public sealed record Destination(string Id, string Glyph, string Label);

    readonly StackPanel _items = new() { Spacing = 4 };
    readonly ContentPresenter _footer = new() { VerticalAlignment = VerticalAlignment.Bottom };
    readonly Dictionary<string, ToggleButton> _buttons = new(StringComparer.Ordinal);
    string _selected = "";

    public event EventHandler<string>? DestinationSelected;

    public UIElement? Footer
    {
        get => _footer.Content as UIElement;
        set => _footer.Content = value;
    }

    public SuiteActivityRail()
    {
        Width = 56;
        Padding = new Thickness(8, 12, 8, 12);
        RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        Children.Add(_items);
        Grid.SetRow(_footer, 1);
        Children.Add(_footer);
        SuiteLandmarks.Navigation(this, "Activity");
    }

    public void SetDestinations(IReadOnlyList<Destination> destinations, string selectedId)
    {
        _items.Children.Clear();
        _buttons.Clear();
        foreach (var destination in destinations)
        {
            var button = MakeButton(destination);
            _buttons[destination.Id] = button;
            _items.Children.Add(button);
        }
        Select(selectedId);
    }

    public void Select(string id)
    {
        if (!_buttons.ContainsKey(id) && _buttons.Count > 0)
            id = _buttons.Keys.First();
        _selected = id;
        foreach (var (key, button) in _buttons)
            button.IsChecked = key == id;
    }

    ToggleButton MakeButton(Destination destination)
    {
        var button = new ToggleButton
        {
            Content = new SuiteIcon(destination.Glyph, SuiteIcon.SizeKind.Rail),
            Width = 40,
            Height = 40,
            Padding = new Thickness(0),
            BorderThickness = new Thickness(0),
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent),
            Tag = destination.Id,
        };
        if (Application.Current.Resources.TryGetValue("SuiteRailButtonStyle", out var style) && style is Style s)
            button.Style = s;
        AutomationProperties.SetName(button, destination.Label);
        ToolTipService.SetToolTip(button, destination.Label);
        button.Click += (_, _) =>
        {
            var id = destination.Id;
            Select(id);
            DestinationSelected?.Invoke(this, id);
        };
        return button;
    }
}
