using CommandLine;
using AppImage_Installer.Verbs;

namespace AppImage_Installer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<
                    InstallVerb,
                    RemoveVerb,
                    ListVerb,
                    UpdateVerb,
                    UpgradeVerb,
                    SearchVerb
                >(args)
                .WithParsed<InstallVerb>(CommandHandlers.HandleInstall)
                .WithParsed<RemoveVerb>(CommandHandlers.HandleRemove)
                .WithParsed<ListVerb>(CommandHandlers.HandleList)
                .WithParsed<UpdateVerb>(CommandHandlers.HandleUpdate)
                .WithParsed<SearchVerb>(CommandHandlers.HandleSearch)
                .WithParsed<UpgradeVerb>(CommandHandlers.HandleUpgrade);
        }
    }
}