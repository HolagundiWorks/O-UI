using System.Data;
using System.Globalization;
using O.Ui.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace O.Ui.Widgets;

/// <summary>
/// Floating suite calculator. Evaluates ordinary arithmetic expressions.
/// Right-click for stay-on-top / transparency / close.
/// </summary>
public sealed class CalculatorWidget : WidgetWindow
{
    static CalculatorWidget? _instance;

    readonly TextBox _expr = new()
    {
        PlaceholderText = "e.g. (10 + 6) * 2.5",
        AcceptsReturn = false,
        MinWidth = 280,
    };
    readonly TextBlock _result = new() { TextWrapping = TextWrapping.Wrap, MinHeight = 24 };

    CalculatorWidget() : base("Calculator")
    {
        AutomationProperties.SetName(_expr, "Expression");
        AutomationProperties.SetLiveSetting(_result, AutomationLiveSetting.Polite);
        AutomationProperties.SetName(_result, "Result");

        var equals = new Button { Content = "=", MinWidth = 48 };
        if (Application.Current?.Resources.TryGetValue("AccentButtonStyle", out var accent) == true && accent is Style style)
            equals.Style = style;
        AutomationProperties.SetName(equals, "Evaluate");
        equals.Click += (_, _) => Evaluate();

        var clear = new Button { Content = "Clear", MinWidth = 72 };
        clear.Click += (_, _) =>
        {
            _expr.Text = "";
            _result.Text = "";
        };

        var keys = new FlexBar();
        foreach (var key in new[] { "7", "8", "9", "+", "4", "5", "6", "-", "1", "2", "3", "*", "0", ".", "/", "(", ")" })
        {
            var token = key;
            var button = new Button { Content = token, MinWidth = 36, MinHeight = 32, Padding = new Thickness(8, 4, 8, 4) };
            AutomationProperties.SetName(button, "Insert " + token);
            button.Click += (_, _) => _expr.Text += token;
            keys.Children.Add(button);
        }

        var row = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = SuiteScale.StackGap,
            Children = { equals, clear },
        };
        var root = new StackPanel
        {
            Spacing = SuiteScale.FieldGap,
            Padding = new Thickness(16),
            Children = { _expr, keys, row, _result },
        };
        AttachChrome(root);
        AppWindow.Resize(new Windows.Graphics.SizeInt32(360, 320));

        _expr.KeyDown += (_, e) =>
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                Evaluate();
            }
        };
    }

    public static void ShowOrFocus()
    {
        if (_instance is null)
        {
            _instance = new CalculatorWidget();
            _instance.Closed += (_, _) => _instance = null;
        }
        _instance.Activate();
    }

    void Evaluate()
    {
        var text = (_expr.Text ?? "").Trim();
        if (text.Length == 0)
        {
            _result.Text = "";
            return;
        }
        try
        {
            // Restrict to arithmetic characters before DataTable.Compute.
            foreach (var ch in text)
            {
                if (!(char.IsDigit(ch) || ch is '.' or '+' or '-' or '*' or '/' or '(' or ')' or ' ' or '\t'))
                {
                    _result.Text = "Use numbers and + − × ÷ ( ) only.";
                    return;
                }
            }
            var value = new DataTable().Compute(text, null);
            _result.Text = Convert.ToDouble(value, CultureInfo.InvariantCulture)
                .ToString("G", CultureInfo.CurrentCulture);
        }
        catch
        {
            _result.Text = "Could not evaluate that expression.";
        }
    }
}
