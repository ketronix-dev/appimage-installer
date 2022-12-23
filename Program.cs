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
                .WithParsed<RemoveVerb>(CommandHandlers.HandleRemove)
                .WithParsed<ListVerb>(CommandHandlers.HandleList)
                .WithParsedAsync<SearchVerb>(CommandHandlers.HandleSearch).Result
                .WithParsedAsync<InstallVerb>(CommandHandlers.HandleInstall).Result
                .WithParsedAsync<UpgradeVerb>(CommandHandlers.HandleUpgrade).Result
                .WithParsed<UpdateVerb>(CommandHandlers.HandleUpdate);
        }
    }
}