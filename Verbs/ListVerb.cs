using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("list", 
    aliases: new []{"l", "-Ls", "-lst"}, 
    HelpText = "List installed apps."
)]
public class ListVerb
{ }