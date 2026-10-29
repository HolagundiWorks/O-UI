using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Hcw.Ui.Controls;

/// <summary>Compact light/dark toggle bound to <see cref="SuiteAppearance"/>.</summary>
public sealed class SuiteThemeSwitch : ToggleButton
{
    Window? _window;

    public SuiteThemeSwitch()
    {
        Content = "Theme";
        MinWidth = 72;
        MinHeight = SuiteScale.ControlMinHeight;
        Padding = new Thickness(8, 2, 8, 2);
        AutomationProperties.SetName(this, "Theme");
        ToolTipService.SetToolTip(this, "Switch light or dark theme");
        Click += OnClick;
        SuiteAppearance.Changed += OnAppearanceChanged;
        Unloaded += (_, _) => SuiteAppearance.Changed -= OnAppearanceChanged;
        Sync();
    }

    /// <summary>Apply the current suite theme to the window and keep it in sync.</summary>
    public void Attach(Window window)
    {
        _window = window;
        SuiteAppearance.Apply(window);
        Sync();
    }

    void OnClick(object sender, RoutedEventArgs e)
    {
        SuiteAppearance.Toggle();
        if (_window is not null)
            SuiteAppearance.Apply(_window);
        Sync();
    }

    void OnAppearanceChanged(ElementTheme _)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            if (_window is not null)
                SuiteAppearance.Apply(_window);
            Sync();
        });
    }

    void Sync()
    {
        IsChecked = SuiteAppearance.IsDark;
        Content = SuiteAppearance.IsDark ? "Dark" : "Light";
        AutomationProperties.SetName(this, SuiteAppearance.IsDark ? "Theme, dark" : "Theme, light");
    }
}
