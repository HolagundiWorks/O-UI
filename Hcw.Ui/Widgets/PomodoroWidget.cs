using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Hcw.Ui.Widgets;

/// <summary>
/// Dial-only Pomodoro: 5–60 minutes in steps of 5 (default 25). Drag the crown to set;
/// click the dial to start/pause. Right-click for stay-on-top / transparency / close.
/// </summary>
public sealed class PomodoroWidget : WidgetWindow
{
    const uint DefaultMinutes = 25;
    const uint MinMinutes = 5;
    const uint MaxMinutes = 60;
    const uint StepMinutes = 5;
    const double DialSize = 160;

    static PomodoroWidget? _instance;

    readonly TextBlock _label = new()
    {
        Text = "25:00",
        FontSize = 22,
        FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
    };
    readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
    readonly Ellipse _track = new()
    {
        Width = DialSize,
        Height = DialSize,
        StrokeThickness = 10,
        Fill = new SolidColorBrush(Colors.Transparent),
    };
    readonly Arc _progress = new() { StrokeThickness = 10 };

    uint _duration = DefaultMinutes * 60;
    uint _left = DefaultMinutes * 60;
    bool _running;
    bool _dragging;
    Point _press;

    PomodoroWidget() : base("Pomodoro")
    {
        AutomationProperties.SetLiveSetting(_label, AutomationLiveSetting.Polite);
        AutomationProperties.SetName(_label, "Remaining time");
        _track.Stroke = BrushOr("BorderSubtleBrush", Colors.Gray);
        _progress.Stroke = BrushOr("BrandAccentBrush", Colors.Orange);

        var dial = new Grid { Width = DialSize, Height = DialSize };
        dial.Children.Add(_track);
        dial.Children.Add(_progress);
        dial.Children.Add(_label);
        AutomationProperties.SetName(dial, "Pomodoro dial");
        dial.PointerPressed += OnDialPressed;
        dial.PointerMoved += OnDialMoved;
        dial.PointerReleased += OnDialReleased;
        dial.PointerCaptureLost += (_, _) => _dragging = false;

        var root = new Grid
        {
            Padding = new Thickness(16),
            Children = { dial },
        };
        AttachChrome(root);
        AppWindow.Resize(new Windows.Graphics.SizeInt32(200, 220));

        _timer.Tick += (_, _) =>
        {
            if (!_running)
                return;
            if (_left == 0)
            {
                _running = false;
                _timer.Stop();
                Paint();
                return;
            }
            _left--;
            Paint();
        };
        Paint();
    }

    public static void ShowOrFocus()
    {
        if (_instance is null)
        {
            _instance = new PomodoroWidget();
            _instance.Closed += (_, _) => _instance = null;
        }
        _instance.Activate();
    }

    void OnDialPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement el)
            return;
        el.CapturePointer(e.Pointer);
        _dragging = true;
        _press = e.GetCurrentPoint(el).Position;
        if (!_running)
            SetFromPoint(el, _press);
        e.Handled = true;
    }

    void OnDialMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_dragging || _running || sender is not UIElement el)
            return;
        SetFromPoint(el, e.GetCurrentPoint(el).Position);
        e.Handled = true;
    }

    void OnDialReleased(object sender, PointerRoutedEventArgs e)
    {
        if (sender is not UIElement el)
            return;
        if (_dragging)
        {
            _dragging = false;
            var point = e.GetCurrentPoint(el).Position;
            el.ReleasePointerCapture(e.Pointer);
            var dx = point.X - _press.X;
            var dy = point.Y - _press.Y;
            if (dx * dx + dy * dy < 36)
                Toggle();
        }
        e.Handled = true;
    }

    void SetFromPoint(UIElement el, Point point)
    {
        if (_running)
            return;
        var cx = DialSize / 2;
        var cy = DialSize / 2;
        var angle = Math.Atan2(point.Y - cy, point.X - cx) * 180 / Math.PI + 90;
        if (angle < 0)
            angle += 360;
        var minutes = (uint)Math.Clamp(
            Math.Round(angle / 360.0 * (MaxMinutes - MinMinutes) / StepMinutes) * StepMinutes + MinMinutes,
            MinMinutes,
            MaxMinutes);
        _duration = minutes * 60;
        _left = _duration;
        Paint();
    }

    void Toggle()
    {
        if (_left == 0)
            _left = _duration;
        _running = !_running;
        if (_running)
            _timer.Start();
        else
            _timer.Stop();
        Paint();
    }

    void Paint()
    {
        var mm = _left / 60;
        var ss = _left % 60;
        _label.Text = $"{mm:00}:{ss:00}";
        var fraction = _duration == 0 ? 0 : (double)_left / _duration;
        _progress.Sweep = Math.Clamp(fraction, 0, 1) * 360;
    }

    static Brush BrushOr(string key, Windows.UI.Color fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Brush brush)
            return brush;
        return new SolidColorBrush(fallback);
    }

    /// <summary>Simple arc path for the dial progress ring.</summary>
    sealed class Arc : Microsoft.UI.Xaml.Shapes.Path
    {
        double _sweep = 360;

        public Arc()
        {
            Width = DialSize;
            Height = DialSize;
            Stretch = Stretch.None;
            IsHitTestVisible = false;
            Stroke = new SolidColorBrush(Colors.Orange);
            Update();
        }

        public double Sweep
        {
            get => _sweep;
            set
            {
                _sweep = value;
                Update();
            }
        }

        void Update()
        {
            const double radius = (DialSize - 10) / 2;
            const double cx = DialSize / 2;
            const double cy = DialSize / 2;
            if (_sweep <= 0.5)
            {
                Data = null;
                return;
            }
            var start = AnglePoint(cx, cy, radius, -90);
            var end = AnglePoint(cx, cy, radius, -90 + _sweep);
            var large = _sweep > 180 ? 1 : 0;
            var figure = new PathFigure { StartPoint = start, IsClosed = false };
            figure.Segments.Add(new ArcSegment
            {
                Point = end,
                Size = new Size(radius, radius),
                SweepDirection = SweepDirection.Clockwise,
                IsLargeArc = large == 1,
            });
            var geo = new PathGeometry();
            geo.Figures.Add(figure);
            Data = geo;
        }

        static Point AnglePoint(double cx, double cy, double r, double deg)
        {
            var rad = deg * Math.PI / 180;
            return new Point(cx + r * Math.Cos(rad), cy + r * Math.Sin(rad));
        }
    }
}
