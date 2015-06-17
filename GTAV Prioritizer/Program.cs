using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GTAV_Prioritizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string GTAVexe = "gta5";
            bool found = false;

            Console.WriteLine("GTAV Prioritizer by Magic");

            while (found == false)
            {

                Console.WriteLine("Scanning for {0}", GTAVexe);
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
                            System.Threading.Thread.Sleep(5000);

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
                    Console.WriteLine("No process found... trying again in 2500ms");
                    System.Threading.Thread.Sleep(2500);
                }
            }

            //and we're done
            Console.WriteLine("Exiting...");
            System.Threading.Thread.Sleep(5000);
            Environment.Exit(1);

        }
    }
}
