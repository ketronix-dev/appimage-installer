using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("install",
    aliases: new []{"i", "-in", "-S"},
    HelpText = "Install the AppImage package.")]
public class InstallVerb
{
    [Value(0, Required = true, HelpText = "Name of a file or package")]
    public string App { get; set; }
}