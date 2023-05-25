using System;
using System.Security;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Microsoft.Win32;


namespace osu_spoofer
{
    class Program
    {
        private static Thread titleThread;
        private static bool stopAnimation = false;

        static void Main(string[] args)
        {

            titleThread = new Thread(TitleAnimation);
            titleThread.Start();


            Console.Write("Do you want to begin process? (Y/N)");
            string a = Console.ReadLine();


            if (a.ToLower() == "n")
            {
                return;
            }
            else if (a.ToLower() != "y" && a.ToLower() == "n")
            {
                return;
            }

            bool keepData = true;
            bool keepSkins = true;
            bool keepSongs = true;

            Console.WriteLine("Deleting osu is required, please continue to do the following.");
            Console.WriteLine("Answer with (Y/N) if you prefer to keep your osu! data.");
            Console.WriteLine("");
            Console.Write("Do you want to keep Data folder? ");
            string data = Console.ReadLine();
            Console.Write("Do you want to keep Skins folder? ");
            string skins = Console.ReadLine();
            Console.Write("Do you want to keep Songs folder? ");
            string songs = Console.ReadLine();

            if (data.ToLower() == "n")
            {
                keepData = false;
            }
            if (skins.ToLower() == "n")
            {
                keepSkins = false;
            }
            if (songs.ToLower() == "n")
            {
                keepSongs = false;
            }

            Console.WriteLine("Keep Data: {0}, Keep Skins: {1}, Keep Songs: {2} ",
                keepData.ToString(), keepSkins.ToString(), keepSongs.ToString());

            Thread.Sleep(1000);

            Console.Clear();
            Thread.Sleep(50);

            Console.WriteLine("Initializing deletion");

            Thread.Sleep(999);

            string userName = Environment.UserName;
            string root = $@"C:\Users\{userName}\AppData\Local\osu!\";
            string dataroot = $@"C:\Users\{userName}\AppData\Local\osu!\Data";
            string skinsroot = $@"C:\Users\{userName}\AppData\Local\osu!\Skins";
            string songsroot = $@"C:\Users\{userName}\AppData\Local\osu!\Songs";


            bool isDataSkipped = false;
            bool isSongsSkipped = false;
            bool isSkinsSkipped = false;

            // If directory does not exist, don't even try   
            if (Directory.Exists(root))
            {
                string[] files = Directory.GetFiles(root);
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"{file} is deleted.");
                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1/3 Successfully deleted all files within main directory");

                string[] directories = Directory.GetDirectories(root);
                Console.WriteLine("2/3 Fetching directories");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (string directory in directories)
                {
                    Console.WriteLine("Ha {0}", directory);

                    if (keepData == true && Directory.Exists(dataroot) && isDataSkipped == false && directory == dataroot) 
                    {
                        Console.WriteLine("Data skipped");
                        isDataSkipped = true;
                    }
                    else if (keepSkins == true && Directory.Exists(skinsroot) && isSkinsSkipped == false && directory == skinsroot)
                    {
                        Console.WriteLine("Skins skipped");
                        isSkinsSkipped = true;
                    }
                    else if (keepSongs == true && Directory.Exists(songsroot) && isSongsSkipped == false && directory == songsroot)
                    {
                        Console.WriteLine("Songs skipped");
                        isSongsSkipped = true;
                    }
                    else
                    {
                        Directory.Delete(directory, true);
                        Console.WriteLine($"{ directory} is deleted.");
                    }
                }
                Thread.Sleep(300);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("3/3 Sucessfully cleaned osu! files");
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(300);
                Console.Clear();
            }

            Spoof spoof = new Spoof();

            Console.WriteLine("Changing osu! registry");
            string hyphenGUID = GenerateRandomGUID();
            string savedUninstallID = spoof.getUninstallID();

            spoof.spoofUninstallID(hyphenGUID);
            Console.WriteLine($"Changed id from {savedUninstallID} to {hyphenGUID}");
            Console.Write("Do you wish to spoof your hardware? (y/n) [possible without] ");
            string spoofState = Console.ReadLine();

            if (spoofState.ToLower() == "y")
            {
                ShowSpoofMenu();
            }
            else
            {
                EndMessage();
                stopAnimation = true;
                Thread.Sleep(100);
                Environment.Exit(0);
            }





        }


        public static void EndMessage()
        {
            Console.WriteLine("you succesfully evaded a ban!");
            Console.WriteLine("make sure to use a vpn when making a new account");
        }

        // Generate GUID in hyphen format
        public static string GenerateRandomGUID()
        {
            Guid guid = Guid.NewGuid();

            string formattedGuid = guid.ToString("D");

            return formattedGuid;
        }

        public static void ShowSpoofMenu()
        {
            Thread.Sleep(100);
            Console.Clear();
            Console.WriteLine("[1] Spoof Disks");
            Console.WriteLine("[2] Spoof Hardware GUID");
            Console.WriteLine("[3] Spoof Machine GUID");
            Console.WriteLine("[4] Spoof Machine ID");
            Console.WriteLine("[5] Spoof All");
            Console.WriteLine("[6] Exit");
            InputMenu();
        }

        public static void InputMenu()
        {
            Spoof spoof = new Spoof();
            Thread.Sleep(100);
            Console.Write("  Select an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    spoof.SpoofDisks();
                    Console.WriteLine("\n  [!] Spoofed Disks");
                    InputMenu();
                    break;
                case "2":
                    spoof.SpoofHwGUID();
                    Console.WriteLine("\n  [!] Spoofed Hardware GUID");
                    InputMenu();
                    break;
                case "3":
                    spoof.SpoofMachineGUID();
                    Console.WriteLine("\n  [!] Spoofed Machine GUID");
                    InputMenu();
                    break;
                case "4":
                    spoof.SpoofMachineID();
                    Console.WriteLine("\n  [!] Spoofed Machine ID");
                    InputMenu();
                    break;
                case "5":
                    spoof.SpoofDisks();
                    spoof.SpoofHwGUID();
                    spoof.SpoofMachineGUID();
                    spoof.SpoofMachineID();
                    Console.WriteLine("\n  [!] Spoofed All");
                    InputMenu();
                    break;
                case "6":
                    Console.WriteLine("\n  [>] Exiting..");
                    Thread.Sleep(100);
                    Console.Clear();
                    EndMessage();
                    stopAnimation = true;
                    Thread.Sleep(100);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\n  [/] Invalid option");
                    InputMenu();
                    break;

            }
        }
        
        public static void TitleAnimation()
        {
            string title = "osu!spoofer | evade bans, evade taxes | justgotripped#7777";
            int currentIndex = 0;
            bool isAdding = true;

            while (!stopAnimation)
            {
                Console.Title = title.Substring(0, currentIndex) + (isAdding ? "_" : "");
                Thread.Sleep(80);

                if (stopAnimation == true)
                {
                    break;
                }

                if (isAdding)
                {
                    currentIndex++;

                    if (currentIndex == title.Length)
                    {
                        isAdding = false;
                    }
                }
                else
                {
                    currentIndex--;

                    if (currentIndex == 0)
                    {
                        isAdding = true;
                    }
                }
            }

        }

        public static void StopAnimation()
        {
            // Request the animation thread to stop
            if (titleThread != null && titleThread.IsAlive)
            {
                titleThread.Abort();
                titleThread.Join();
            }
        }
    }
}

