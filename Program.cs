using CommandLine;
using AppImage_Installer.Verbs;

namespace AppImage_Installer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<
                    InstallVerb
                >(args)
                .WithParsed<InstallVerb>(CommandHandlers.HandleInstall);
        }
    }
}