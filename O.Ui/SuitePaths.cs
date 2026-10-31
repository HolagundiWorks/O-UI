namespace O.Ui;

/// <summary>Shared paths under %LOCALAPPDATA% for the O-Studio suite hub.</summary>
public static class SuitePaths
{
    public const string LocalFolder = "O-Studio";

    public static string File(string name) =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LocalFolder, name);
}
