using System;
using System.IO;
public class Debug
{
    /// <summary>
    /// Loads the debug shit, creates log files etc etc 
    /// </summary>
    public static void Init()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory() + "\\logs\\");
        //check if old logs exist and clear em
        try
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine("Log directory already exists... continuing");
            }
            else
            {
                Console.WriteLine("Creating log directory...");
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + "debug.log"))
            {
                foreach (FileInfo f in new DirectoryInfo(path).GetFiles("debug*.log"))
                {
                    f.Delete();
                    File.Create(path + f.ToString()).Close();
                    Console.WriteLine(f.ToString() + " deleted and remade.");
                }
            }
            else
            {
                File.Create(path + "debug_normal.log").Close();
                File.Create(path + "debug_error.log").Close();
                File.Create(path + "debug_special.log").Close();
                File.Create(path + "debug.log").Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;

        }
    }
    /// <summary>
    /// Outputs to the console with a timestamp before the message, and writes all this to a file. [string, bool, bool, bool]
    /// Usage: ConOut(Message, Error?, Special?, Really Special?)
    /// </summary>
    public static void ConOut(string Msg, bool ERR = false, bool SPC = false, bool SPC2 = false)
    {

        if (ERR)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": ERROR: " + Msg);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Message);

            LogToFile(Message, "error");
        }
        else if (SPC)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": SPECIAL: " + Msg);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Message);

            LogToFile(Message, "special");
        }
        else if (SPC2)
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": SPECIAL: " + Msg);


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Message);

            LogToFile(Message, "special");
        }
        else
        {
            string Message = (DateTime.Now.Hour + ":" + DateTime.Now.Minute + "." + DateTime.Now.Second + "|" + DateTime.Now.Millisecond + ": " + Msg);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message);

            LogToFile(Message, "normal");
        }
    }

    private static void LogToFile(string toWrite, string type)
    {
        try
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory() + "\\logs\\");

            StreamReader FileToRead_All = new StreamReader(FilePath + "debug.log"); //this writes to a single file

            StreamReader FileToRead = new StreamReader(FilePath + "debug_" + type + ".log");//this writes to type-based log file

            string toWriteAppended = FileToRead.ReadToEnd();
            toWriteAppended += toWrite + Environment.NewLine;

            string toWriteAppended_All = FileToRead_All.ReadToEnd();
            toWriteAppended_All += toWrite + Environment.NewLine;

            FileToRead.Close();
            FileToRead_All.Close();


            StreamWriter FileToWrite_All = new StreamWriter(FilePath + "debug.log");

            StreamWriter FileToWrite = new StreamWriter(FilePath + "debug_" + type + ".log");
            FileToWrite.Write(toWriteAppended);
            FileToWrite_All.Write(toWriteAppended_All);

            FileToWrite.Close();
            FileToWrite_All.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

}
