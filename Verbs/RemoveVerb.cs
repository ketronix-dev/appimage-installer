using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("remove", 
    aliases: new []{"-rm", "r", "-R"}, 
    HelpText = "Remove the AppImage package."
    )]
public class RemoveVerb
{
    [Value(0, Required = true, HelpText = "Name of a file or package")]
    public string App { get; set; }
}