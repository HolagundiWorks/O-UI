using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Hcw.Ui;

/// <summary>Gives a navigation pane the suite's look in the app's accent. System controls keep their own colours unless the resources are set on the control itself, so this does that.</summary>
public static class SuiteNavigation
{
    public static void Apply(NavigationView navigation, SuiteApp app)
    {
        navigation.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"ms-appx:///Hcw.Ui/Themes/SuiteNavigation.{SuiteBrand.Accent(app)}.xaml") });
    }
}
