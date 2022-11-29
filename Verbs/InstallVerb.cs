using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("install", HelpText = "Install the AppImage package.")]
public class InstallVerb
{
    [Value(0, Required = true, HelpText = "Name of a file or package")]
    public string App { get; set; }
}