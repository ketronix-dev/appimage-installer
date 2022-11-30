using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("update", 
    aliases: new []{"up", "-Ur"}, 
    HelpText = "Update repository database"
)]
public class UpdateVerb
{ }