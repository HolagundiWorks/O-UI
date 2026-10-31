namespace O.Ui;

/// <summary>The apps of the suite. Each has its own name and accent colour.</summary>
public enum SuiteApp
{
    /// <summary>O-Studio: the office. Orange.</summary>
    Studio,

    /// <summary>O-CoreOp: quantities and costing. Blue.</summary>
    Metra,

    /// <summary>O-Draft: drafting. Black.</summary>
    Draft,
}

/// <summary>The names of the suite and its apps, so every app and document spells them the same way.</summary>
public static class SuiteBrand
{
    public const string Short = "OSAS";
    public const string FullName = "Open Source Architecture Suit";

    public static string Name(SuiteApp app) => app switch
    {
        SuiteApp.Studio => "O-Studio",
        SuiteApp.Metra => "O-CoreOp",
        _ => "O-Draft",
    };

    /// <summary>What the app is for, in a few words.</summary>
    public static string Purpose(SuiteApp app) => app switch
    {
        SuiteApp.Studio => "Office and project management",
        SuiteApp.Metra => "Quantities, estimates and billing",
        _ => "Drafting",
    };

    /// <summary>The name of the accent: the middle part of the SuiteAccent.* and SuiteLight.* file names.</summary>
    public static string Accent(SuiteApp app) => app switch
    {
        SuiteApp.Studio => "Orange",
        SuiteApp.Metra => "Blue",
        _ => "Black",
    };
}
