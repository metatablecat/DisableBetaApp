using System;
using System.Diagnostics;
using System.IO;

namespace DisableBetaApp
{
    internal class Program
    {
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

            var startInfo = new ProcessStartInfo
            {
                FileName = Exec,
                Arguments = launchargs,
                UseShellExecute = true
            };

            Process.Start(startInfo); 
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Roblox...");
            FindAndLaunchRoblox(args, args.Length == 1);
        }
    }
}
