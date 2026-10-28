using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Hcw.Ui;

/// <summary>Optional titled body host for a rail page (launcher uses body-only sheets).</summary>
public sealed class SuiteContentSheet : StackPanel
{
    readonly TextBlock _title = new();
    readonly ContentPresenter _body = new();

    public SuiteContentSheet()
    {
        Spacing = SuiteScale.FieldGap;
        if (Application.Current.Resources.TryGetValue("SectionTitleStyle", out var style) && style is Style s)
            _title.Style = s;
        _title.Visibility = Visibility.Collapsed;
        Children.Add(_title);
        Children.Add(_body);
    }

    public string Title
    {
        get => _title.Text;
        set
        {
            _title.Text = value ?? "";
            _title.Visibility = string.IsNullOrWhiteSpace(value) ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public UIElement? Body
    {
        get => _body.Content as UIElement;
        set => _body.Content = value;
    }
}
