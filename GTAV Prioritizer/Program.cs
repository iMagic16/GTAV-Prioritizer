using System;
using System.Diagnostics;
using System.IO;

namespace GTAV_Prioritizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Grand Theft Auto Prioritizer by Magic");

            try
            {
                /*
                if (!(File.Exists("MagiCorpDevTools.dll")))
                {
                    File.WriteAllBytes("MagiCorpDevTools.dll", Properties.Resources.MagiCorpDevTools_Debugger);
                }
                */


                Debug.Init();

                bool skiptheBS = false;
                skiptheBS = Properties.Settings.Default.SkipBS;


                bool RGSC_Exists;
                if (!(Properties.Settings.Default.GTARGSC == "true" | Properties.Settings.Default.GTARGSC == "false"))
                {
                    RGSC_Exists = false;
                }
                else
                {
                    RGSC_Exists = true;
                }

                if (File.Exists("noskip.txt"))
                {
                    skiptheBS = false;
                    Debug.ConOut("noskip.txt found...", false, true);
                    Configure(skiptheBS);
                }
                else if (!RGSC_Exists)
                {
                    Debug.ConOut("Old version? resetting config", true);
                    skiptheBS = false; //if using old config......
                    Configure(skiptheBS);
                }
                else if (skiptheBS)
                {
                    Debug.ConOut("Program already configured, continuing...");
                }
                else
                {
                    Debug.ConOut("Program not yet configured, configuring...");
                    Configure(false);
                }
            }
            catch (Exception e)
            {
                Debug.ConOut(e.Message, true);
            }


            //rungta!
            bool LaunchGTAornah = Properties.Settings.Default.LaunchGTA;
            if (LaunchGTAornah == true)
            {
                if (Properties.Settings.Default.GTARGSC == "true")
                {
                    LaunchGTA(true);
                }
                else
                {
                    LaunchGTA(false);
                }
            }

            //prioritize it!
            Prioritize();

            //close launcher!
            if (Properties.Settings.Default.CloseGTALauncher == true)
                CloseLauncher();

            //we're done
            Debug.ConOut("Program Finished, exiting...");
            System.Threading.Thread.Sleep(10000);
            Environment.Exit(1);



        }

        static void Configure(bool SkipTheBS)
        {
            if (SkipTheBS == false)
            {
                //should we close launcher
                string ToCloseLauncher = "";
                while (!(ToCloseLauncher == "y" | ToCloseLauncher == "n"))
                {
                    Debug.ConOut("Do you wish to close GTAVLauncher.exe when the game is loaded? y/n (may cause crashing)", false, false, true);
                    try
                    {
                        ToCloseLauncher = Console.ReadLine();
                    }
                    catch (Exception) { }

                }

                if (ToCloseLauncher == "y")
                {
                    Properties.Settings.Default.CloseGTALauncher = true;
                }
                else
                {
                    Properties.Settings.Default.CloseGTALauncher = false;
                }

                string StartGTA = "";
                //should we start gta from here
                while (!(StartGTA == "y" | StartGTA == "n"))
                {
                    Debug.ConOut("Do you wish to start gtav every time this program is launched? y/n", false, false, true);

                    try
                    {
                        StartGTA = Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Debug.ConOut(e.Message);
                    }

                    if (StartGTA == "y")
                    {
                        Properties.Settings.Default.LaunchGTA = true;
                    }
                    else
                    {
                        Properties.Settings.Default.LaunchGTA = false;
                    }

                }

                string RGSC = "";
                while (!(RGSC == "r" | RGSC == "s"))
                {

                    Debug.ConOut("Are you using Steam or RGSC? r/s", false, false, true);
                    RGSC = Console.ReadLine();
                    if (RGSC == "r")
                    {
                        Properties.Settings.Default.GTARGSC = "true";
                    }
                    else if (RGSC == "s")
                    {
                        Properties.Settings.Default.GTARGSC = "false";
                    }
                }

                Debug.ConOut("Program configured, if you wish to change some settings create a file called noskip.txt where this program is.", false, true);
                Properties.Settings.Default.SkipBS = true;


            }
            Properties.Settings.Default.Save();

        }


        static void CloseLauncher()
        {

            //wait for game to load social club
            Debug.ConOut("Waiting for game to load...");
            System.Threading.Thread.Sleep(10000);

            string GTAVLauncherexe = "GTAVLauncher";
            Debug.ConOut("Attempting to close launcher...");
            Process[] RunningShit = Process.GetProcessesByName(GTAVLauncherexe);
            if (RunningShit.Length > 0)
            {
                foreach (Process GTAVLauncher in RunningShit)
                {
                    Debug.ConOut("Process found. [ Name: " + GTAVLauncher.ProcessName + " | ID: " + GTAVLauncher.Id + " | Prio: " + GTAVLauncher.PriorityClass + " ]",false, true);

                    Debug.ConOut("Killing...");
                    try
                    {
                        GTAVLauncher.Kill();
                        Debug.ConOut("Process killed", false, true);
                    }
                    catch (Exception e)
                    {
                        Debug.ConOut(e.Message, true);
                    }
                }
            }
            else
            {
                Debug.ConOut("No GTAVlauncher found...");
            }

        }
        static void LaunchGTA(bool isRGSC)
        {
            //steam://run/271590
            Debug.ConOut("Booting GTA");
            if (!isRGSC)
            {
                try
                {
                    Debug.ConOut("Launching via Steam");
                    Process.Start("steam://run/271590");

                }
                catch (Exception e)
                {
                    Debug.ConOut(e.Message);
                }
            }
            else
            {
                try
                {
                    Debug.ConOut("Launching via RGSC");
                    Debug.ConOut("PLEASE MAKE SURE THIS EXE IS IN THE SAME LOCATION AS GTA5.EXE");
                    Process.Start("PlayGTAV.exe");

                }
                catch (Exception e)
                {
                    Debug.ConOut(e.Message, true);
                }
            }
        }
        static void Prioritize()
        {
            string GTAVexe = "gta5";
            bool found = false;

            while (found == false)
            {

                Debug.ConOut("Scanning for " + GTAVexe + ".exe");
                System.Threading.Thread.Sleep(500);
                //check for GTAV.exe
                Process[] RunningProcess = Process.GetProcessesByName(GTAVexe);
                if (RunningProcess.Length > 0)
                {
                    foreach (Process GTAVProcess in RunningProcess)
                    {
                        try
                        {
                            Debug.ConOut("Process found. [ Name: " + GTAVProcess.ProcessName + " | ID: " + GTAVProcess.Id + " | Prio: " + GTAVProcess.PriorityClass + " ]",false, true);
                            //wait for game to chillout
                            Debug.ConOut("Waiting for game to settle after boot...");
                            System.Threading.Thread.Sleep(15000);

                            Debug.ConOut("Setting prio to high..");
                            //set to high prio
                            System.Threading.Thread.Sleep(500);
                            try
                            {
                                GTAVProcess.PriorityClass = ProcessPriorityClass.High;
                                Debug.ConOut("Priority set");
                                System.Threading.Thread.Sleep(500);
                                GTAVProcess.Refresh();
                                Debug.ConOut("Process found. [ Name: " + GTAVProcess.ProcessName + " | ID: " + GTAVProcess.Id + " | Prio: " + GTAVProcess.PriorityClass + " ]", false, true);
                                System.Threading.Thread.Sleep(500);

                                found = true;
                            }
                            catch (Exception e)
                            {
                                Debug.ConOut(e.Message, true);
                            
                            System.Threading.Thread.Sleep(5000);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.ConOut(e.Message, true);
                        
                        System.Threading.Thread.Sleep(5000);
                        }
                    }
                }
                else
                {
                    Debug.ConOut("No process found... trying again in 5000ms");
                    System.Threading.Thread.Sleep(5000);
                }
            }


        }
    }

}
