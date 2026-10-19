// Draws the suite's marks and writes PNG, ICO and lockup files for each app:   dotnet run --project tools/Brand -- <output folder>
// The mark is the O-CoreOp logo redrawn as geometry, so it can be made in any colour at any size: a half ring open to the right, a smaller
// half ring below, and a ring at the centre, white on a plate in the app's colour.
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

var output = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "brand");
var apps = new (string Folder, string Name, Color Plate)[]
{
    ("studio", "O-Studio", ColorTranslator.FromHtml("#FF6B00")),
    ("coreop", "O-CoreOp", ColorTranslator.FromHtml("#1F5FD1")),
    ("draft", "O-Draft", ColorTranslator.FromHtml("#17181B")),
    ("osas", "OSAS", ColorTranslator.FromHtml("#17181B")),
};
var ink = ColorTranslator.FromHtml("#17181B");
var inkSoft = ColorTranslator.FromHtml("#45484D");

foreach (var (folder, name, plate) in apps)
{
    var dir = Path.Combine(output, folder);
    Directory.CreateDirectory(dir);
    var suite = folder == "osas"; // the suite's own mark has the three accents as a strip along the bottom
    Mark(1024, plate, suite: suite).Save(Path.Combine(dir, "mark.png"), ImageFormat.Png);
    Mark(256, plate, suite: suite).Save(Path.Combine(dir, "mark-256.png"), ImageFormat.Png);
    Ico(Path.Combine(dir, "app.ico"), plate, suite);
    Lockup(name, plate, suite).Save(Path.Combine(dir, "lockup.png"), ImageFormat.Png);
    File.WriteAllText(Path.Combine(dir, "mark.svg"), Svg(name, plate, suite));
    if (suite)
        continue;

    var tiles = Path.Combine(dir, "tiles");
    Directory.CreateDirectory(tiles);
    foreach (var (file, size) in new[] { ("Square44x44Logo.png", 44), ("Square71x71Logo.png", 71), ("Square150x150Logo.png", 150), ("Square310x310Logo.png", 310), ("StoreLogo.png", 50), ("logo.png", 150), ("logo_plate.png", 256) })
        Mark(size, plate, full: file != "logo_plate.png").Save(Path.Combine(tiles, file), ImageFormat.Png);
    Wide(310, 150, plate).Save(Path.Combine(tiles, "Wide310x150Logo.png"), ImageFormat.Png);
    Wide(620, 300, plate).Save(Path.Combine(tiles, "SplashScreen.png"), ImageFormat.Png);
}
Console.WriteLine("Written to " + output);

// The same mark as geometry in a 194 by 194 box: a half ring open to the right (radius 69, stroke 18), a smaller half ring below
// the centre (radius 50, stroke 12), and a ring at the centre (radius 18.5, stroke 5), all white, centred at (107, 96).
static string Svg(string name, Color plate, bool suite)
{
    var fill = $"#{plate.R:X2}{plate.G:X2}{plate.B:X2}";
    var strip = suite
        ? "  <rect x=\"0\" y=\"177\" width=\"64.7\" height=\"17\" fill=\"#FF6B00\"/>\n  <rect x=\"64.7\" y=\"177\" width=\"64.7\" height=\"17\" fill=\"#1F5FD1\"/>\n  <rect x=\"129.3\" y=\"177\" width=\"64.7\" height=\"17\" fill=\"#C9C6BC\"/>\n"
        : "";
    return $"""
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 194 194" width="194" height="194">
  <title>{name}</title>
  <rect width="194" height="194" fill="{fill}"/>
{strip}  <g fill="none" stroke="#fff">
    <path d="M107 27A69 69 0 0 0 107 165" stroke-width="18"/>
    <path d="M57 96A50 50 0 0 0 157 96" stroke-width="12"/>
    <circle cx="107" cy="96" r="18.5" stroke-width="5"/>
  </g>
</svg>
""";
}

static Bitmap NewBitmap(int w, int h) => new(w, h, PixelFormat.Format32bppArgb);

static Graphics Canvas(Bitmap bitmap)
{
    var g = Graphics.FromImage(bitmap);
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
    g.Clear(Color.Transparent);
    return g;
}

// The glyph is drawn in the coordinates of the original 256 pixel plate (plate 31..224); the transform fits the plate to the target square.
static void DrawMark(Graphics g, float x, float y, float size, Color plate, bool full, bool suite = false)
{
    var state = g.Save();
    var s = size / 194f;
    g.TranslateTransform(x, y);
    g.ScaleTransform(s, s);
    g.TranslateTransform(-31, -31);
    using (var brush = new SolidBrush(plate))
    {
        if (full)
        {
            using var path = Rounded(31, 31, 194, 194, 34);
            g.FillPath(brush, path);
        }
        else
            g.FillRectangle(brush, 31, 31, 194, 194);
    }
    if (suite)
    {
        using var clip = Rounded(31, 31, 194, 194, 34);
        var saved = g.Save();
        g.SetClip(clip);
        var colours = new[] { ColorTranslator.FromHtml("#FF6B00"), ColorTranslator.FromHtml("#1F5FD1"), ColorTranslator.FromHtml("#C9C6BC") };
        for (var i = 0; i < 3; i++)
        {
            using var stripe = new SolidBrush(colours[i]);
            g.FillRectangle(stripe, 31 + i * 194f / 3f, 208, 194f / 3f + 1, 17);
        }
        g.Restore(saved);
    }
    using var white = new Pen(Color.White) { StartCap = LineCap.Flat, EndCap = LineCap.Flat };
    white.Width = 18;
    g.DrawArc(white, 138 - 69, 127 - 69, 138, 138, 90, 180); // the C: a half ring open to the right
    white.Width = 12;
    g.DrawArc(white, 138 - 50, 127 - 50, 100, 100, 0, 180); // the smaller half ring below the centre
    white.Width = 5;
    g.DrawEllipse(white, 138 - 18.5f, 127 - 18.5f, 37, 37); // the ring at the centre
    g.Restore(state);
}

static GraphicsPath Rounded(float x, float y, float w, float h, float r)
{
    var path = new GraphicsPath();
    path.AddArc(x, y, 2 * r, 2 * r, 180, 90);
    path.AddArc(x + w - 2 * r, y, 2 * r, 2 * r, 270, 90);
    path.AddArc(x + w - 2 * r, y + h - 2 * r, 2 * r, 2 * r, 0, 90);
    path.AddArc(x, y + h - 2 * r, 2 * r, 2 * r, 90, 90);
    path.CloseFigure();
    return path;
}

static Bitmap Mark(int size, Color plate, bool full = true, bool suite = false)
{
    var bitmap = NewBitmap(size, size);
    using var g = Canvas(bitmap);
    DrawMark(g, 0, 0, size, plate, full, suite);
    return bitmap;
}

static Bitmap Wide(int w, int h, Color plate)
{
    var bitmap = NewBitmap(w, h);
    using var g = Canvas(bitmap);
    using (var brush = new SolidBrush(plate))
        g.FillRectangle(brush, 0, 0, w, h);
    var size = h * 0.62f;
    // the plate is the same colour as the background, so only the white glyph shows
    DrawMark(g, (w - size) / 2, (h - size) / 2, size, plate, full: false);
    return bitmap;
}

static Bitmap Lockup(string name, Color plate, bool suite = false)
{
    var bitmap = NewBitmap(1600, 640);
    using var g = Canvas(bitmap);
    DrawMark(g, 40, 40, 560, plate, full: true, suite: suite);
    using var title = new Font("Segoe UI Semibold", 150, FontStyle.Regular, GraphicsUnit.Pixel);
    using var line = new Font("Segoe UI", 52, FontStyle.Regular, GraphicsUnit.Pixel);
    using var small = new Font("Segoe UI Semibold", 44, FontStyle.Regular, GraphicsUnit.Pixel);
    using var inkBrush = new SolidBrush(ColorTranslator.FromHtml("#17181B"));
    using var softBrush = new SolidBrush(ColorTranslator.FromHtml("#45484D"));
    using var accentBrush = new SolidBrush(plate);
    g.DrawString(name, title, inkBrush, 650, 150);
    if (suite)
    {
        g.DrawString("Open Source Architecture Suit", line, softBrush, 660, 360);
        g.DrawString("O-Studio · O-CoreOp · O-Draft", small, softBrush, 660, 440);
        return bitmap;
    }
    g.DrawString("Open Source Architecture Suit", line, softBrush, 660, 360);
    g.DrawString("OSAS", small, accentBrush, 660, 440);
    return bitmap;
}

// An ICO file whose images are PNGs, which Windows has read since Vista.
static void Ico(string path, Color plate, bool suite = false)
{
    var sizes = new[] { 16, 24, 32, 48, 64, 128, 256 };
    var images = sizes.Select(s =>
    {
        using var bitmap = Mark(s, plate, suite: suite);
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        return stream.ToArray();
    }).ToList();
    using var file = File.Create(path);
    using var w = new BinaryWriter(file);
    w.Write((ushort)0);
    w.Write((ushort)1);
    w.Write((ushort)sizes.Length);
    var offset = 6 + 16 * sizes.Length;
    for (var i = 0; i < sizes.Length; i++)
    {
        w.Write((byte)(sizes[i] == 256 ? 0 : sizes[i]));
        w.Write((byte)(sizes[i] == 256 ? 0 : sizes[i]));
        w.Write((byte)0);
        w.Write((byte)0);
        w.Write((ushort)1);
        w.Write((ushort)32);
        w.Write(images[i].Length);
        w.Write(offset);
        offset += images[i].Length;
    }
    foreach (var image in images)
        w.Write(image);
}
