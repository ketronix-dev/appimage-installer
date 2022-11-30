using CommandLine;

namespace AppImage_Installer.Verbs;

[Verb("search", 
    aliases: new []{"-Sr", "s", "-srh"}, 
    HelpText = "Search the AppImage package."
)]
public class SearchVerb
{
    [Value(0, Required = true, HelpText = "Name of a file or package")]
    public string App { get; set; }
}