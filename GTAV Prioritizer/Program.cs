﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace GTAV_Prioritizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Grand Theft Auto Prioritizer by Magic");
            bool skiptheBS = false;
            skiptheBS = Properties.Settings.Default.SkipBS;
            if (File.Exists("noskip.txt"))
            {
                skiptheBS = false;
                Console.WriteLine("noskip.txt found...");
            }
            else if (skiptheBS)
            {
                Console.WriteLine("Program already configured, continuing...");
            }
            else
            {
                Console.WriteLine("Program not yet configured, configuring...");
            }

            #region configuration
            if (skiptheBS == false)
            {
                //should we close launcher
                string ToCloseLauncher = "";
                while (!(ToCloseLauncher == "y" | ToCloseLauncher == "n"))
                {
                    Console.WriteLine("Do you wish to close GTAVLauncher.exe when the game is loaded? y/n (may cause crashing)");
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
                    Console.WriteLine("Do you wish to start gtav every time this program is launched? y/n");
                    
                    try
                    {
                        StartGTA = Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
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

                    Console.WriteLine("Are you using Steam or RGSC? r/s");
                    RGSC = Console.ReadLine();
                    if (RGSC == "r")
                    {
                        Properties.Settings.Default.GTARGSC = true;
                    }
                    else if (RGSC == "s")
                    {
                        Properties.Settings.Default.GTARGSC = false;
                    }
                }

                Console.WriteLine("Program configured, if you wish to change some settings create a file called noskip.txt where this program is.");
                Properties.Settings.Default.SkipBS = true;


            }
            Properties.Settings.Default.Save();
            #endregion

            //rungta!
            bool LaunchGTAornah = Properties.Settings.Default.LaunchGTA;
            if (LaunchGTAornah)
                LaunchGTA(Properties.Settings.Default.GTARGSC);

            //prioritize it!
            Prioritize();

            //close launcher!
            if (Properties.Settings.Default.CloseGTALauncher == true)
                CloseLauncher();

            //we're done
            Console.WriteLine("Program Finished, exiting...");
            System.Threading.Thread.Sleep(10000);
            Environment.Exit(1);
        }
        static void CloseLauncher()
        {

            //wait for game to load social club
            Console.WriteLine("Waiting for game to load...");
            System.Threading.Thread.Sleep(10000);

            string GTAVLauncherexe = "GTAVLauncher";
            Console.WriteLine("Attempting to close launcher...");
            Process[] RunningShit = Process.GetProcessesByName(GTAVLauncherexe);
            if (RunningShit.Length > 0)
            {
                foreach (Process GTAVLauncher in RunningShit)
                {
                    Console.WriteLine("Process found. [ Name: {0} | ID: {1} | Prio: {2} ]", GTAVLauncher.ProcessName, GTAVLauncher.Id, GTAVLauncher.PriorityClass);

                    Console.WriteLine("Killing...");
                    try
                    {
                        GTAVLauncher.Kill();
                        Console.WriteLine("Process killed");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("No GTAVlauncher found...");
            }

        }
        static void LaunchGTA(bool isRGSC)
        {
            //steam://run/271590
            Console.WriteLine("Booting GTA");
            if (!isRGSC)
            {
                try
                {
                    Console.WriteLine("Launching via Steam");
                    System.Diagnostics.Process.Start("steam://run/271590");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Console.WriteLine("Launching via RGSC");
                    Console.WriteLine("PLEASE MAKE SURE THIS EXE IS IN THE SAME LOCATION AS GTA5.EXE");
                    System.Diagnostics.Process.Start("PlayGTAV.exe");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static void Prioritize()
        {
            string GTAVexe = "gta5";
            bool found = false;

            while (found == false)
            {

                Console.WriteLine("Scanning for {0}.exe", GTAVexe);
                System.Threading.Thread.Sleep(500);
                //check for GTAV.exe
                Process[] RunningProcess = Process.GetProcessesByName(GTAVexe);
                if (RunningProcess.Length > 0)
                {
                    foreach (Process GTAVProcess in RunningProcess)
                    {
                        try
                        {
                            Console.WriteLine("Process found. [ Name: {0} | ID: {1} | Prio: {2} ]", GTAVProcess.ProcessName, GTAVProcess.Id, GTAVProcess.PriorityClass);
                            //wait for game to chillout
                            Console.WriteLine("Waiting for game to settle after boot...");
                            System.Threading.Thread.Sleep(15000);

                            Console.WriteLine("Setting prio to high..");
                            //set to high prio
                            System.Threading.Thread.Sleep(500);
                            try
                            {
                                GTAVProcess.PriorityClass = ProcessPriorityClass.High;
                                Console.WriteLine("Priority set");
                                System.Threading.Thread.Sleep(500);
                                GTAVProcess.Refresh();
                                Console.WriteLine("Process updated. [ Name: {0} | ID: {1} | Prio: {2} ]", GTAVProcess.ProcessName, GTAVProcess.Id, GTAVProcess.PriorityClass);
                                System.Threading.Thread.Sleep(500);

                                found = true;
                            }
                            catch (Exception err)
                            {
                                Console.WriteLine(err.Message);
                                System.Threading.Thread.Sleep(5000);
                            }
                        }
                        catch (Exception er)
                        {
                            Console.WriteLine(er.Message);
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No process found... trying again in 5000ms");
                    System.Threading.Thread.Sleep(5000);
                }
            }


        }
    }

}
