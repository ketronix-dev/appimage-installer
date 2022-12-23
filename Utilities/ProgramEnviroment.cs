using ServiceClasses;
using Pastel;

namespace Utilities
{
    public class ProgramEnvironment
    {
        public static void CheckDirectories()
        {
            var userHome = Environment.GetEnvironmentVariable("HOME");
        
            if (Directory.Exists($"{userHome}/.local/share/apps") == false)
            {
                Directory.CreateDirectory($"{userHome}/.local/share/apps");
            }

            if (Directory.Exists($"{userHome}/.local/share/apps/cache") == false)
            {
                Directory.CreateDirectory($"{userHome}/.local/share/apps/cache");
            }
            if (Directory.Exists($"{userHome}/.local/share/apps/app_icons") == false)
            {
                Directory.CreateDirectory($"{userHome}/.local/share/apps/app_icons");
            }
            if (Directory.Exists($"{userHome}/.local/share/apps/app_images") == false)
            {
                Directory.CreateDirectory($"{userHome}/.local/share/apps/app_images");
            }
        }
        public static void ShowAppInfo( App app,bool update)
        {
            if(update != true)
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Found the package you requested".Pastel("#42f569"));
            }
            else
            {
                Console.WriteLine("Found the package for update".Pastel("#42f569"));
            }
            Console.WriteLine($"Name: {app.Name}".Pastel("#edea15"));
            Console.WriteLine($"Name of package: {app.ShortName}".Pastel("#edea15"));
            Console.WriteLine($"Maintainer: {app.Maintainer}".Pastel("#edea15"));
            Console.WriteLine($"Source: {app.NewVersionSources}".Pastel("#edea15"));
            Console.WriteLine($"Version: {app.Version}".Pastel("#edea15"));
            Console.WriteLine("-------------------------------");
        }

        public static bool AskAccept(string state, string package)
        {
            Console.Write($"Confirm the {state} of this {package}: [Y|n] ");
            string read = Console.ReadLine();
            if(read == "y" || read == "Y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}