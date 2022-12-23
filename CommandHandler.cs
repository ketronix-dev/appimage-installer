using AppImage_Installer.Verbs;
using Mono.Unix;
using Pastel;
using ServiceClasses;
using Utilities;

namespace AppImage_Installer
{
    internal class CommandHandlers
    {
        internal static async Task HandleInstall(InstallVerb installVerb)
        {
            Console.WriteLine($"Install app: {installVerb.App}");
            if(File.Exists(installVerb.App))
            {
                if(ProgramEnvironment.AskAccept("install", "package"))
                {
                    var ufi = new UnixFileInfo(installVerb.App);
                    ufi.FileAccessPermissions |= FileAccessPermissions.UserExecute;

                    Console.WriteLine("Begin installing app...".Pastel("#42f569"));
                    Integrate.InstallApp(installVerb.App);
                    Console.WriteLine("Done".Pastel("#42f569"));
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                if(File.Exists($"{$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/repo.base"))
                {
                    // Console.WriteLine($"Request: {args[1]}");
                    if(CloudRepoUtils.checkApp(CloudRepoUtils.Connection, installVerb.App))
                    {
                        await Service.InstallFromCloudRepo(installVerb.App);
                    }
                    else
                    {
                        Console.WriteLine("Maybe you made a mistake when entering, because there is no such file.".Pastel("#edea15"));
                    }
                }
                else
                {
                    Console.WriteLine("If you want to install an application from a remote repository, start by downloading or updating the base with the \"app update\" command.".Pastel("#edea15"));
                }
            }
        }
        internal static void HandleRemove(RemoveVerb removeVerb)
        {
            Console.WriteLine($"Remove app: {removeVerb.App}");
            
            if(ProgramEnvironment.AskAccept("remove", "package"))
            {
                Console.WriteLine("Begin removing app...".Pastel("#42f569"));
                Integrate.RemoveApp(removeVerb.App);
                Console.WriteLine("Done".Pastel("#42f569"));
            }
            else
            {
                Environment.Exit(0);
            }
        }
        internal static async Task HandleSearch(SearchVerb searchVerb)
        {
            Console.WriteLine($"Search app: {searchVerb.App}");
            Service.SearchAppInCloudRepo(searchVerb.App);
        }
        internal static void HandleList(ListVerb listVerb)
        {
            Service.ListInstalledApps();
        }
        internal static void HandleUpdate(UpdateVerb updateVerb)
        {
            Service.UpdateLocalRepoBase();
        }
        internal static async Task HandleUpgrade(UpgradeVerb upgradeVerb)
        {
            await Service.UpgradeAllApp();
        }
    }
}