using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;

namespace O.Ui.Widgets;

/// <summary>Floating suite widget base: stay-on-top, opacity, and a right-click menu.</summary>
public abstract class WidgetWindow : Window
{
    const int HwndTopmost = -1;
    const int HwndNotopmost = -2;
    const uint SwpNosize = 0x0001;
    const uint SwpNomove = 0x0002;
    const uint SwpNoactivate = 0x0010;

    bool _topmost = true;
    double _opacity = 1.0;

    protected WidgetWindow(string title)
    {
        Title = title;
        Closed += (_, _) => OnClosed();
    }

    protected void AttachChrome(FrameworkElement root)
    {
        Content = root;
        root.RequestedTheme = SuiteAppearance.Current;
        SuiteAppearance.Changed += OnTheme;
        var menu = new MenuFlyout();
        var top = new ToggleMenuFlyoutItem { Text = "Stay on top", IsChecked = _topmost };
        top.Click += (_, _) =>
        {
            _topmost = top.IsChecked;
            ApplyTopmost();
        };
        var dim = new MenuFlyoutItem { Text = "Transparency" };
        dim.Click += (_, _) => CycleOpacity(root);
        var close = new MenuFlyoutItem { Text = "Close" };
        close.Click += (_, _) => Close();
        menu.Items.Add(top);
        menu.Items.Add(dim);
        menu.Items.Add(new MenuFlyoutSeparator());
        menu.Items.Add(close);
        root.ContextFlyout = menu;
        ApplyTopmost();
    }

    void OnTheme(ElementTheme theme)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            if (Content is FrameworkElement root)
                root.RequestedTheme = theme;
        });
    }

    void OnClosed() => SuiteAppearance.Changed -= OnTheme;

    void CycleOpacity(FrameworkElement root)
    {
        _opacity = _opacity switch
        {
            >= 0.95 => 0.75,
            >= 0.70 => 0.55,
            _ => 1.0,
        };
        root.Opacity = _opacity;
    }

    void ApplyTopmost()
    {
        try
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            SetWindowPos(hwnd, _topmost ? HwndTopmost : HwndNotopmost, 0, 0, 0, 0,
                SwpNomove | SwpNosize | SwpNoactivate);
        }
        catch
        {
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
}
