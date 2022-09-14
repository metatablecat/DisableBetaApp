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

        static void FindAndLaunchRoblox(string[] args, bool playGame)
        {
            string launchargs;
            if (playGame == true) {
                launchargs = args[0]
                .Replace("+LaunchExp:InApp", "+LaunchExp:InBrowser");
            } else {
                launchargs = "--play";
            }

            string LocalAppDataEnv = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
            string[] RobloxExecutable = Directory.GetFiles(LocalAppDataEnv + "\\Roblox\\Versions", "RobloxPlayerLauncher.exe", SearchOption.AllDirectories);
            string Exec = RobloxExecutable[0];

            // we need to set the key back to its old value temporarily to trick Roblox into think it's installed
            SetLaunchArg(Exec);

            var startInfo = new ProcessStartInfo
            {
                FileName = Exec,
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
