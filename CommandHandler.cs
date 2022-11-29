using System;
using System.Linq;
using System.Text;
using AppImage_Installer.Verbs;

namespace AppImage_Installer
{
    internal class CommandHandlers
    {
        internal static void HandleInstall(InstallVerb installVerb)
        {
            Console.WriteLine(installVerb.App);
        }
    }
}