using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("upgrade", 
    aliases: new []{"upg", "-Up"}, 
    HelpText = "Check and install updates for apps."
)]
public class UpgradeVerb
{ }