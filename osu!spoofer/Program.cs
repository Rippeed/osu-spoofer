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
        private static readonly bool isDebugging = false; // default false 

        public static string Title = @"                 _                        __          " + Environment.NewLine +
            @"                | |                      / _|         " + Environment.NewLine +
            @"  ___  ___ _   _| |___ _ __   ___   ___ | |_ ___ _ __ " + Environment.NewLine +
            @" / _ \/ __| | | | / __| '_ \ / _ \ / _ \|  _/ _ \ '__|" + Environment.NewLine +
            @"| (_) \__ \ |_| |_\__ \ |_) | (_) | (_) | ||  __/ |   " + Environment.NewLine +
            @" \___/|___/\__,_(_)___/ .__/ \___/ \___/|_| \___|_|   " + Environment.NewLine +
            @"                      | |                             " + Environment.NewLine +
            @"                      |_|                             " + Environment.NewLine;

        static void Main(string[] args)
        {


            titleThread = new Thread(TitleAnimation);
            titleThread.Start();


            StartingSession();

            bool keepData = true;
            bool keepSkins = true;
            bool keepSongs = true;

            Console.WriteLine("\nDeleting osu is required, please continue to do the following.");
            Console.WriteLine("Answer with (Y/N) if you prefer to keep your osu! data.");
            Console.WriteLine("");
            Console.Write("Do you want to keep Data folder?  ");
            string data = Console.ReadLine();
            Console.Write("Do you want to keep Skins folder?  ");
            string skins = Console.ReadLine();
            Console.Write("Do you want to keep Songs folder?  ");
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
                    if (isDebugging == false)
                    {
                        File.Delete(file);
                        Console.WriteLine($"{file} is deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"{file} is deleted. [ DEBUGGING ]");
                    }

                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1/3 Successfully deleted all files within main directory");

                string[] directories = Directory.GetDirectories(root);
                Console.WriteLine("2/3 Fetching directories");
                Console.ForegroundColor = ConsoleColor.Gray;
                
                foreach (string directory in directories)
                {

                    if (keepData == true && Directory.Exists(dataroot) && isDataSkipped == false && directory == dataroot) 
                    {
                        Console.WriteLine("Folder Data Saved!");
                        isDataSkipped = true;
                    }
                    else if (keepSkins == true && Directory.Exists(skinsroot) && isSkinsSkipped == false && directory == skinsroot)
                    {
                        Console.WriteLine("Folder Skisn Saved!");
                        isSkinsSkipped = true;
                    }
                    else if (keepSongs == true && Directory.Exists(songsroot) && isSongsSkipped == false && directory == songsroot)
                    {
                        Console.WriteLine("Folder Songs Saved!");
                        isSongsSkipped = true;
                    }
                    else
                    {
                        if (isDebugging == false)
                        {
                            Directory.Delete(directory, true);
                            Console.WriteLine($"{ directory} is deleted.");
                        }
                        else
                        {
                            Console.WriteLine($"{ directory} is deleted. [ DEBUGGING ]");
                        }
                    }
                }
                Thread.Sleep(300);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("3/3 Sucessfully cleaned osu! files");
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(300);
                //Console.Clear();
            }

            Spoof spoof = new Spoof();

            Console.WriteLine("\nChanging osu! registry");
            string hyphenGUID = GenerateRandomGUID();
            string savedUninstallID = spoof.getUninstallID();

            spoof.spoofUninstallID(hyphenGUID);
            Console.WriteLine($"Changed id from {savedUninstallID} to {hyphenGUID}");
            
            for (int i=1; i < 5; i++)
            {
                Console.WriteLine("Continuing in {0}", i.ToString());
                Thread.Sleep(999);
                
                if (i == 4)
                {
                    ShowSpoofMenu();
                }
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
            Console.WriteLine(Title);
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
                    Thread.Sleep(2800);
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

        public static void StartingSession()
        {
            Console.WriteLine("Starting process in 5 seconds.");
            Thread.Sleep(800);
            Console.Clear();
            Console.WriteLine("Starting process in 4 seconds..");
            Thread.Sleep(798);
            Console.Clear();
            Console.WriteLine("Starting process in 3 seconds...");
            Thread.Sleep(758);
            Console.Clear();
            Console.WriteLine("Starting process in 2 seconds.");
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Starting process in 1 second..");
            Thread.Sleep(500);
            Console.WriteLine("Starting...");
        }
    }
}

