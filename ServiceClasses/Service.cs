using System.Net;
using ShellProgressBar;
using Mono.Unix;
using Pastel;
using Utilities;

namespace ServiceClasses
{
    class Service
    {
        public static async Task DownloadAsync(string Link, string DownloadTo)
        {
            try
            {
                var options = new ProgressBarOptions
                {
                    ProgressCharacter = '█',
                    ProgressBarOnBottom = true
                };
                const int totalTicks = 100;
                using (WebClient wc = new WebClient())
                {
                    var pbar = new ProgressBar(totalTicks, "Initial message", options);
                    wc.DownloadProgressChanged += (s, e) => { pbar.Tick(e.ProgressPercentage,$"Download file {e.BytesReceived}/{e.TotalBytesToReceive}"); };
                    await wc.DownloadFileTaskAsync(new Uri(Link), DownloadTo);
                };
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error downloading the file, try turning on the VPN, or restarting your network connection");
                System.Environment.Exit(1);
            }
        }

        public static void UpdateLocalRepoBase()
        {
            Console.WriteLine("Begin update database.".Pastel("#42f569"));
                if(!Directory.Exists($"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"))
                {
                    Directory.CreateDirectory($"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps");
                }
                if(File.Exists($"{$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/repo.base"))
                {
                    File.Delete($"{$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/repo.base");
                }
                var webClient = new WebClient();
                try
                {
                    webClient.DownloadFile("http://mirror.osel.pp.ua/repo.base", $"{$"{Environment.GetEnvironmentVariable("HOME")}/.local/share/apps"}/repo.base");
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error downloading the file, try turning on the VPN, or restarting your network connection");
                    System.Environment.Exit(1);
                }
                Console.WriteLine("Database download success");
                Console.WriteLine("Done".Pastel("#11f569"));
        }

        public static async Task InstallFromCloudRepo(string name, bool update = false)
        {
            var userHome = Environment.GetEnvironmentVariable("HOME");
            App? appToInstall = CloudRepoUtils.ReadAppInfo(CloudRepoUtils.Connection, name);
            
            ProgramEnvironment.CheckDirectories();

            var dest = $"{userHome}/.local/share/apps/cache/app.AppImage";
            
            ProgramEnvironment.ShowAppInfo(appToInstall, update);
            
            if(ProgramEnvironment.AskAccept())
            {
                Console.WriteLine("Begin downloading...".Pastel("#42f569"));
                await Service.DownloadAsync(appToInstall.Link, dest);
                Console.WriteLine("\n \n");
                Console.WriteLine("Enable permissions...".Pastel("#42f569"));

                var ufi = new UnixFileInfo(dest);
                ufi.FileAccessPermissions |= FileAccessPermissions.UserExecute;
                                    
                Console.WriteLine("Begin installing...".Pastel("#42f569"));
                Integrate.InstallApp(dest, appToInstall.Version);
                Console.WriteLine("Done".Pastel("#42f569"));
                File.Delete(dest);
                Console.CursorVisible = true;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public static async Task UpgradeAllApp()
        {
            var Apps = DataBaseUtils.GetAllApps(DataBaseUtils.Connection);
            var AppsToUpdate = new List<string>();

            foreach (var app in Apps)
            {
                if(CloudRepoUtils.checkApp(CloudRepoUtils.Connection, app.AppName))
                {
                    if(app.Version != CloudRepoUtils.ReadAppInfo(CloudRepoUtils.Connection, app.AppName).Version)
                    {
                        AppsToUpdate.Add(app.AppName);
                    }
                }
            }
            if(AppsToUpdate.Count != 0)
            {
                Console.WriteLine("Apps to upgrade:".Pastel("#42f569"));

                Console.WriteLine("-------------------------------");
                foreach (var name in AppsToUpdate)
                {
                    Console.WriteLine(name.Pastel("#edea15"));
                }
                Console.WriteLine("-------------------------------");
                
                if(ProgramEnvironment.AskAccept())
                {
                    foreach (var name in AppsToUpdate)
                    {
                        Integrate.RemoveApp(name, true);
                        await InstallFromCloudRepo(name, true);
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("No updates avaiable.".Pastel("#42f569"));
            }
        }
        public static void ListInstalledApps()
        {
            var apps = DataBaseUtils.GetAllApps(DataBaseUtils.Connection);
            var appsInstall = new List<string>();

            if(apps.Count != 0)
            {
                foreach (var app in apps)
                {
                    appsInstall.Add(app.AppName);
                }
            }

            if(appsInstall.Count != 0)
            {
                Console.WriteLine("Installed apps:".Pastel("#42f569"));

                Console.WriteLine("-------------------------------");
                foreach (var name in appsInstall)
                {
                    Console.WriteLine(name.Pastel("#edea15"));
                }
                Console.WriteLine("-------------------------------");
            }
            else
            {
                Console.WriteLine("No updates avaiable.".Pastel("#42f569"));
            }
        }
        public static void SearchAppInCloudRepo(string name)
        {
            if(CloudRepoUtils.checkApp(CloudRepoUtils.Connection, name))
            {
                var app = CloudRepoUtils.ReadAppInfo(CloudRepoUtils.Connection, name);
                ProgramEnvironment.ShowAppInfo(app, false);
            }
        }
    }
}