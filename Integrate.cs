using Mono.Unix;
using Utilities;
using Pastel;
using Utilities;

public class Integrate
{
    public static void InstallApp(string pathToAppImage, string version = "n")
    {
        Console.WriteLine("Reading the Path...".Pastel("#1f7d34"));
        
        var userHome = Environment.GetEnvironmentVariable("HOME");
        var folderInPath = Path.GetDirectoryName(pathToAppImage);

        Console.WriteLine("Unpacking the package...".Pastel("#1f7d34"));

        ProgramEnvironment.CheckDirectories();

        Environment.CurrentDirectory = $"{userHome}/.local/share/apps/cache";
        try
        {
            CoreCommands.ExecShellCommand(pathToAppImage, "--appimage-extract");
        }
        catch (System.Exception)
        {
            Console.WriteLine("File execution error, try installing libfuse, or update libappimage");
            System.Environment.Exit(1);
        }
        var squashfsRoot = $"{Environment.CurrentDirectory}/squashfs-root";

        Console.WriteLine("Search for the required files...".Pastel("#1f7d34"));
        
        var desktopfiles = Directory.GetFiles(squashfsRoot, "*.desktop");
        var pngfiles = Directory.GetFiles(squashfsRoot, "*.png");
        var Content = File.ReadAllText(desktopfiles[0]).Split('\n');

        Console.WriteLine("Package name generation...".Pastel("#1f7d34"));
        
        var nameApp = Content[FindIndexOfWord(Content, "Name")]
            .Split('=')[1]
            .Replace(" ", String.Empty).ToLower();

        var copyPngTo =
            $"{userHome}/.local/share/apps/app_icons/{nameApp}.png";
        var copyAppImageTo =
            $"{userHome}/.local/share/apps/app_images/{nameApp}.AppImage";
        var newDesktopFile =
            $"{userHome}/.local/share/applications/{nameApp}.desktop";

        var ufi = new UnixFileInfo(newDesktopFile);
        
        Console.WriteLine("Checking directories...".Pastel("#1f7d34"));

        Console.WriteLine("Copying files...".Pastel("#1f7d34"));
        
        File.Copy(pngfiles[0], copyPngTo, true);
        File.Copy(pathToAppImage, copyAppImageTo, true);

        Console.WriteLine("Creating a \".desktop\" file...".Pastel("#1f7d34"));
        
        Content[FindIndexOfWord(Content, "Exec")] = $"Exec={copyAppImageTo}";
        Content[FindIndexOfWord(Content, "Icon")] = $"Icon={copyPngTo}";
        
        File.WriteAllLines(desktopfiles[0], Content);
        
        File.Copy(desktopfiles[0], newDesktopFile, true);
        
        Console.WriteLine("Setting access parameters...".Pastel("#1f7d34"));
        
        ufi.FileAccessPermissions |= FileAccessPermissions.UserExecute;
        Directory.Delete(squashfsRoot, true);
        try
        {
            DataBaseUtils.CreateTableOrNo(DataBaseUtils.Connection);
        }
        catch (Exception)
        {}

        DataBaseUtils.InsertApp(DataBaseUtils.Connection, nameApp, newDesktopFile, copyAppImageTo, version);
        
        Console.WriteLine($"The application is installed, to remove it later - specify the phrase \"{nameApp}\" instead of the file path, and use \"remove\" instead of \"install\".".Pastel("#edea15"));
    }

    public static void RemoveApp(string appName, bool NoOutput = false)
    {
        if(NoOutput != true)
        {
            Console.WriteLine("Searching for an app...".Pastel("#1f7d34"));
        }
        if (DataBaseUtils.checkApp(DataBaseUtils.Connection, appName))
        {
            var App = DataBaseUtils.ReadAppInfo(DataBaseUtils.Connection, appName);
            if(NoOutput != true)
            {
                Console.WriteLine("Removing ALL traces of the application...".Pastel("#1f7d34"));
            }
            File.Delete(App.DesktopFile);
            File.Delete(App.AppImageFile);
            DataBaseUtils.DeleteApp(DataBaseUtils.Connection, appName);
        }
        else
        {
            Console.WriteLine("The application is not installed!".Pastel("#edea15"));
        }
    }
    
    public static int FindIndexOfWord(string[] array, string word) => 
        array
            .Select((str, i) => (str.Split('='), i))
            .Where(x => x.Item1.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)))
            .Select(x => x.Item2)
            .Cast<int?>()
            .FirstOrDefault() ?? -1;
}