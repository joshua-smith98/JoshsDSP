using SysConsole = System.Console;
using JoshsDSP.Core;

namespace JoshsDSP.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            bool IsRunning = true;
            string? cmd;

            while (IsRunning)
            {
                SysConsole.Write(">");
                cmd = SysConsole.ReadLine();

                //Ignore command if null (should never be true)
                if (cmd is null) continue;

                //Split args by space
                string[] cmdArgs = cmd.Split(' ');
                if (cmdArgs.Length <= 0) continue;

                string concat;

                //TBI (eventually): an actual command system; this current one is really basic
                switch (cmdArgs[0])
                {
                    case "play":
                        concat = string.Join(' ', cmdArgs.Skip(1));
                        if (!GlobalDSP.PlayFile(concat)) SysConsole.WriteLine("Failed to play file.");
                        break;
                    case "stop":
                        if (cmdArgs.Length > 1) SysConsole.WriteLine("Expected zero arguments.");
                        else if (!GlobalDSP.Stop()) SysConsole.WriteLine("Failed to stop playing.");
                        break;
                    case "exit":
                        IsRunning = false;
                        break;
                    default:
                        SysConsole.WriteLine("Command not recognised.");
                        break;
                }
            }
        }
    }
}
