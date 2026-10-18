# Hcw.Ui

The shared design of the HolagundiWorks desktop apps. At the moment AOS (the office app) and AQC (quantities and costing) use it. It is a WinUI 3 class library for .NET 8 with no licence chosen yet; that is the owner's decision, and AQC is AGPL-3.0, so it matters before this is published.

## What is in it
| File | What it does |
|---|---|
| `Themes/SuiteColors.xaml` | The colours, in one place: warm paper, ink, and the amber of the product mark. Change a colour here and both apps follow |
| `Themes/SuiteLight.xaml`, `SuiteHighContrast.xaml` | The light theme (app brushes plus the overrides the controls need) and the high-contrast theme |
| `Themes/SuiteStyles.xaml` | Text styles (page title, section title, body, secondary, caption, table header and cell), defaults for buttons, text boxes and combo boxes, the amber primary button, a card and a chip |
| `Themes/SuiteNavigation.xaml`, `SuiteNavigation.cs` | The look of the left navigation pane: ink indicator, amber wash on the selected page |
| `Controls/FlexBar.cs` | A row of buttons that wraps when the page is narrow |

Amber is for fills and marks. Use `SuiteAmberText` for amber text; amber on white is too faint to read. Contrast on white: ink 17.2, ink soft 8.9, amber text 7.0; ink on amber 11.6.

## Using it
Reference the project, then in `App.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <XamlControlsResources xmlns="using:Microsoft.UI.Xaml.Controls" />
    <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteColors.xaml"/>
    <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteStyles.xaml"/>
</ResourceDictionary.MergedDictionaries>
<ResourceDictionary.ThemeDictionaries>
    <ResourceDictionary x:Key="Light">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteLight.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
    <ResourceDictionary x:Key="HighContrast">
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="ms-appx:///Hcw.Ui/Themes/SuiteHighContrast.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</ResourceDictionary.ThemeDictionaries>
```

and on each navigation pane: `Hcw.Ui.SuiteNavigation.Apply(navigationView);`. A system control keeps its own colours unless they are set on the control, which is why the pane needs that call.

## Structure the apps share
A menu bar, a left navigation pane with a heading for each group of pages, and the page beside it. AOS and AQC both follow this. The library holds the look; each app keeps its own pages.

## Building
The apps reference this project by relative path (`..\..\..\hcw-ui\Hcw.Ui\Hcw.Ui.csproj`), so check it out beside them. It targets Windows App SDK 1.6, the lowest version either app uses.
