using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;

namespace Hcw.Ui;

/// <summary>Title + supporting line for a suite stage page.</summary>
public sealed class SuitePageHeader : StackPanel
{
    readonly TextBlock _title = new();
    readonly TextBlock _support = new() { TextWrapping = TextWrapping.Wrap, MaxWidth = 720 };

    public SuitePageHeader()
    {
        Spacing = SuiteScale.StackGap;
        if (Application.Current.Resources.TryGetValue("PageTitleStyle", out var titleStyle) && titleStyle is Style ts)
            _title.Style = ts;
        if (Application.Current.Resources.TryGetValue("SecondaryStyle", out var supportStyle) && supportStyle is Style ss)
            _support.Style = ss;
        AutomationProperties.SetHeadingLevel(_title, AutomationHeadingLevel.Level1);
        Children.Add(_title);
        Children.Add(_support);
    }

    public string Title
    {
        get => _title.Text;
        set => _title.Text = value ?? "";
    }

    public string Support
    {
        get => _support.Text;
        set
        {
            _support.Text = value ?? "";
            _support.Visibility = string.IsNullOrWhiteSpace(value) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
