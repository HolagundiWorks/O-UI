# O-UI

WinUI 3 class library **`O.Ui`** (folder `o-ui/O.Ui/`). **HCW.ui / `Hcw.Ui` is retired** — use this repository only. The shared design of **OSAS, the Open Source Architecture Suit**: colours, text styles, control defaults, marks, suite shell chrome, and a few layout controls. User-visible app names and file terms match **`docs/SUITE.md`** in any suite app repo (section 7). O-Studio (the office app) and O-CoreOp (quantities, estimates and billing) use it now; the OSAS launcher uses the activity-rail shell; O-Draft (drafting) is meant to follow. Targets .NET 8 under the MIT licence (the marks in `brand/` are separate, see `brand/LICENSE.md`). MIT code can be used inside O-CoreOp, which is AGPL-3.0, and inside O-Draft, which is MIT or Apache-2.0.

| App | Accent |
|---|---|
| O-Studio | Orange `#FF6B00`, ink text on it |
| O-CoreOp | Blue `#1F5FD1`, white text on it |
| O-Draft | Black `#1B1C20`, white text on it |

## What is in it
| File | What it does |
|---|---|
| `Themes/SuiteColors.xaml`, `SuiteColors.Dark.xaml` | Light and dark neutral palettes |
| `Themes/SuiteAccent.Orange.xaml`, `.Blue.xaml`, `.Black.xaml` | The accent of each app: fill, hover, pressed, text, selection wash, and the text colour on a fill |
| `Themes/SuiteLight.<Accent>.xaml`, `SuiteDark.<Accent>.xaml`, `SuiteHighContrast.xaml` | Light/dark theme dictionaries per accent, plus high contrast. Generated from `Themes/templates` by `tools/make-variants.mjs` |
| `Themes/SuiteStyles.xaml` | Text styles, control defaults, `SuiteCardGap`, `SuiteRailButtonStyle`, card and chip |
| `Themes/SuiteNavigation.<Accent>.xaml`, `SuiteNavigation.cs` | The look of the left navigation pane |
| `SuiteBrand.cs` | Suite and app names (`SuiteApp.Studio`, `.Metra` = O-CoreOp assembly alias, `.Draft`) |
| `SuiteLandmarks.cs` | Draft-parity landmark helpers (`Navigation` / `Main` / `status`) |
| `SuiteShell.cs`, `SuiteActivityRail.cs`, `SuitePageHeader.cs`, `SuiteContentSheet.cs` | Launcher-style rail + stage chrome (landmarks built in) |
| `SuiteIcon.cs`, `SuiteIcons.cs`, `SuiteScale.cs` | Shared glyphs and spacing |
| `SuiteAppearance.cs`, `SuiteMotion.cs`, `Controls/SuiteThemeSwitch.cs` | Shared light/dark preference (`%LOCALAPPDATA%\OSAS\appearance.json`) and motion preference |
| `Widgets/PomodoroWidget.cs`, `Widgets/CalculatorWidget.cs` | Floating suite widgets (`ShowOrFocus`) |
| `brand/`, `tools/Brand` | Marks; run `dotnet run --project tools/Brand -- brand` to redraw |
| `Controls/FlexBar.cs` | A row of buttons that wraps when the page is narrow |

Accent colours are for fills and marks. Use `SuiteAccentText` for text in the accent colour. Contrast on white: ink 17.2, ink soft 8.9. Orange text 6.2, blue text 7.7. Ink on orange 6.3, white on blue 5.8.

## Using it
Reference the project, then in `App.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
    <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteColors.xaml"/>
    <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteAccent.Orange.xaml"/>  <!-- or .Blue, .Black -->
    <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteStyles.xaml"/>
</ResourceDictionary.MergedDictionaries>
<ResourceDictionary.ThemeDictionaries>
    <ResourceDictionary x:Key="Light">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteLight.Orange.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    <ResourceDictionary x:Key="Default">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteDark.Orange.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    <ResourceDictionary x:Key="Dark">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteDark.Orange.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    <ResourceDictionary x:Key="HighContrast">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///O.Ui/Themes/SuiteHighContrast.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

On each navigation pane: `O.Ui.SuiteNavigation.Apply(navigationView, O.Ui.SuiteApp.Studio);`. For the launcher rail/stage host: `new SuiteShell()` (landmarks applied on rail and stage). Theme: `SuiteThemeSwitch.Attach(window)` or `SuiteAppearance.Apply(window)`.

## Structure the apps share
A menu bar, a left navigation pane (or activity rail) with destinations, and the page beside it. O-Studio and O-CoreOp use NavigationView; the OSAS launcher uses `SuiteShell` / `SuiteActivityRail`. The library holds the look; each app keeps its own pages.

## Building
The apps reference this project by relative path (`..\..\..\o-ui\O.Ui\O.Ui.csproj`), so clone `HolagundiWorks/O-UI` beside them as the folder name `o-ui`. The assembly name remains `O.Ui`. It targets Windows App SDK 1.6, the lowest version either app uses.

## Licence
The code is MIT licensed (`LICENSE`). The marks in `brand/` are not; see `brand/LICENSE.md`.
