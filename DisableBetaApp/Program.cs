using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace DisableBetaApp
{
    internal class Program
    {

        static RegistryKey GetSubKey(RegistryKey key, params string[] path)
        {
            string constructedPath = Path.Combine(path);
            return key.CreateSubKey(constructedPath, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
        }

        static void SetLaunchArg(string root)
        {
            const string _ = "";
            RegistryKey Classes = GetSubKey(Registry.CurrentUser, "SOFTWARE", "Classes");
            RegistryKey Protocol = GetSubKey(Classes, "roblox-player");
            Protocol.SetValue(_, "URL: Roblox Protocol");
            Protocol.SetValue("URL Protocol", _);

            RegistryKey ProtocolCmd = GetSubKey(Protocol, "shell", "open", "command");
            ProtocolCmd.SetValue(_, "\"" + root + "\" %1");
        }

        static string GetVersion()
        {
            return (string)GetSubKey(
                Registry.CurrentUser,
                "SOFTWARE",
                "ROBLOX Corporation",
                "Environments",
                "roblox-player"
            ).GetValue("version");
        }

        static void FindAndLaunchRoblox(string[] args, bool playGame)
        {
            string launchargs;
            if (playGame == true) {
                launchargs = args[0]
                .Replace("+LaunchExp:InApp", "+LaunchExp:InBrowser");
            } else {
                launchargs = "--play";
            }

            // better workflow, grab onto the version number
            // should fix some issues with updating
            var ClientVersion = GetVersion();

            string LocalAppDataEnv = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
            string AppFolder = LocalAppDataEnv + "\\Roblox\\Versions\\" + ClientVersion;
            string RobloxExecutable =  AppFolder + "\\RobloxPlayerLauncher.exe";

            // we need to set the key back to its old value temporarily to trick Roblox into thinking it's installed
            SetLaunchArg(RobloxExecutable);

            var startInfo = new ProcessStartInfo
            {
                FileName = RobloxExecutable,
                Arguments = launchargs,
                UseShellExecute = true
            };

            var proc = Process.Start(startInfo);
            proc.WaitForExit();

            // now we can change the key to this program
            Console.Write("Setting startup registry keys...");
            SetLaunchArg(System.Environment.ProcessPath);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Roblox...");
            FindAndLaunchRoblox(args, args.Length == 1);
        }
    }
}
