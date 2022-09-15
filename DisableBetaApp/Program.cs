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

        static void SetLaunchReg(string root)
        {
            const string _ = "";
            RegistryKey Classes = GetSubKey(Registry.CurrentUser, "SOFTWARE", "Classes");
            RegistryKey Protocol = GetSubKey(Classes, "roblox-player");
            Protocol.SetValue(_, "URL: Roblox Protocol");
            Protocol.SetValue("URL Protocol", _);

            RegistryKey ProtocolCmd = GetSubKey(Protocol, "shell", "open", "command");
            ProtocolCmd.SetValue(_, "\"" + root + "\" %1");

            // set protocol image
            RegistryKey Icon = GetSubKey(Protocol, "DefaultIcon");
            Icon.SetValue(_, root);
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
        
        static string? FindLauncherPath(string installFolder)
        {
            foreach (string i in Directory.EnumerateDirectories(installFolder, "version-*"))
            {
                string path = i + "\\RobloxPlayerLauncher.exe";
                if (File.Exists(path))
                {
                    return path;
                }
            }
        }

        static string FindRoblox()
        {
            string ProgramFilesEnv = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            string ProgramFilesFolder = ProgramFilesEnv + "\\Roblox\\Versions";
            if (FindLauncherPath(ProgramFilesFolder) is string path)
            {
                return path;
            }
            
            string LocalAppDataEnv = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
            string LocalAppDataFolder = LocalAppDataEnv + "\\Roblox\\Versions";
            if (FindLauncherPath(LocalAppDataFolder) is string path)
            {
                return path;
            }
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
            
            // we need to set the key back to its old value temporarily to trick Roblox into thinking it's installed
            string RobloxExecutable = FindRoblox();
            SetLaunchReg(RobloxExecutable);

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
            SetLaunchReg(System.Environment.ProcessPath);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Roblox...");
            FindAndLaunchRoblox(args, args.Length == 1);
        }
    }
}
