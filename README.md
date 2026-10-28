# Hcw.Ui

The shared design of **OSAS, the Open Source Architecture Suit**: its colours, text styles, control defaults, marks and a few layout controls. O-Studio (the office app) and O-CoreOp (quantities, estimates and billing) use it now; O-Draft (drafting) is meant to follow. It is a WinUI 3 class library for .NET 8 under the MIT licence (the marks in `brand/` are separate, see `brand/LICENSE.md`). MIT code can be used inside O-CoreOp, which is AGPL-3.0, and inside O-Draft, which is MIT or Apache-2.0.

| App | Accent |
|---|---|
| O-Studio | Orange `#FF6B00`, ink text on it |
| O-CoreOp | Blue `#1F5FD1`, white text on it |
| O-Draft | Black `#1B1C20`, white text on it |

## What is in it
| File | What it does |
|---|---|
| `Themes/SuiteColors.xaml` | The neutral colours, in one place: warm paper and ink |
| `Themes/SuiteAccent.Orange.xaml`, `.Blue.xaml`, `.Black.xaml` | The accent of each app: fill, hover, pressed, text, selection wash, and the text colour on a fill |
| `Themes/SuiteLight.<Accent>.xaml`, `SuiteHighContrast.xaml` | The light theme in that accent (app brushes plus the overrides the controls need), and the high-contrast theme. Generated from `Themes/templates` by `tools/make-variants.mjs` |
| `Themes/SuiteStyles.xaml` | Text styles (page title, section title, body, secondary, caption, table header and cell), defaults for buttons, text boxes and combo boxes, the primary button in the accent colour, a card and a chip |
| `Themes/SuiteNavigation.<Accent>.xaml`, `SuiteNavigation.cs` | The look of the left navigation pane: ink indicator, accent wash on the selected page |
| `SuiteBrand.cs` | The names of the suite and its apps (`SuiteApp.Studio`, `.Metra`, `.Draft`) |
| `SuiteLandmarks.cs` | Draft-parity landmark helpers (`Navigation` / `Main` / `status`) for app shells until SuiteShell lands in this package |
| `brand/`, `tools/Brand` | The marks: the O-CoreOp logo redrawn as geometry in each app's colour, as PNG, ICO, a lockup and the Windows tiles. Run `dotnet run --project tools/Brand -- brand` to draw them again |
| `Controls/FlexBar.cs` | A row of buttons that wraps when the page is narrow |

Accent colours are for fills and marks. Use `SuiteAccentText` for text in the accent colour. Contrast on white: ink 17.2, ink soft 8.9. Orange text 6.2, blue text 7.7. Ink on orange 6.3, white on blue 5.8.

## Using it
Reference the project, then in `App.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
    <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteColors.xaml"/>
    <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteAccent.Orange.xaml"/>  <!-- or .Blue, .Black -->
    <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteStyles.xaml"/>
</ResourceDictionary.MergedDictionaries>
<ResourceDictionary.ThemeDictionaries>
    <ResourceDictionary x:Key="Light">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteLight.Orange.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    <ResourceDictionary x:Key="HighContrast">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteHighContrast.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

and on each navigation pane: `Hcw.Ui.SuiteNavigation.Apply(navigationView, Hcw.Ui.SuiteApp.Studio);`. A system control keeps its own colours unless they are set on the control, which is why the pane needs that call.

## Structure the apps share
A menu bar, a left navigation pane with a heading for each group of pages, and the page beside it. O-Studio and O-CoreOp both follow this. The library holds the look; each app keeps its own pages.

## Building
The apps reference this project by relative path (`..\..\..\hcw-ui\Hcw.Ui\Hcw.Ui.csproj`), so check it out beside them. It targets Windows App SDK 1.6, the lowest version either app uses.

## Licence
The code is MIT licensed (`LICENSE`). The marks in `brand/` are not; see `brand/LICENSE.md`.
