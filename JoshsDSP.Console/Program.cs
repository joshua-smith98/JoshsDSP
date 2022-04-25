using SysConsole = System.Console;
using JoshsDSP.Core;

#region Local Methods

/// <summary>
/// Parses a command string and splits arguments using whitespace, except when contained within quotes
/// </summary>
string[] getArgs(string cmd)
{
    string[] cmdSplit = cmd.Split(' ');
    List<string> cmdArgs = new();

    bool isInQuotes = false;
    int argCounter = 0;

    foreach (string arg in cmdSplit)
    {
        if (arg.Contains('\"')) isInQuotes = !isInQuotes;

        //if (cmdArgs.Count <= argCounter) cmdArgs.Add(arg.ToLower()); //Commands are not case sensitive
        if (cmdArgs.Count <= argCounter) cmdArgs.Add(arg); //Nevermind - we need commands to be case sensitive so the DSP names make sense
        else cmdArgs[argCounter] += $" {arg}";

        if (!isInQuotes) argCounter++;
    }

    return cmdArgs.ToArray();
}

/// <summary>
/// Parses a command string and returns the remainder of the string, staring from an argument index.
/// </summary>
string getRemainderFrom(string[] args, int index)
{
    string[] lastArgs = new string[args.Length - index];

    for (int i = 0; i < lastArgs.Length; i++) lastArgs[i] = args[index + i];

    return string.Join(' ', lastArgs);
}

#endregion

#region Main

bool IsRunning = true;
string? cmd;
string dir = Directory.GetCurrentDirectory();

while (IsRunning)
{
    SysConsole.Write(">");
    cmd = SysConsole.ReadLine();

    //Ignore command if null (should never be true) or if it is empty
    if (cmd is null || cmd == string.Empty) continue;

    string[] cmdArgs = getArgs(cmd);

    //TBI (eventually): an actual command system; this current one is really basic
    switch (cmdArgs[0])
    {
        case "get":
            {
                if (cmdArgs.Length < 2)
                {
                    SysConsole.WriteLine("'get' expects a subcommand.");
                    break;
                }

                switch (cmdArgs[1])
                {
                    case "position":
                        {
                            if (!GlobalDSP.IsPlaying)
                                SysConsole.WriteLine("No file is currently playing.");
                            else
                                SysConsole.WriteLine(GlobalDSP.Position);
                            break;
                        }
                    default:
                        {
                            SysConsole.WriteLine("Subcommand not recognised.");
                            break;
                        }
                }
                
                break;
            }
        case "set":
            {
                if (cmdArgs.Length < 3)
                    SysConsole.WriteLine("'set' expects a subcommand and a parameter.");
                else
                    switch (cmdArgs[1])
                    {
                        case "position":
                            {
                                GlobalDSP.Position = Convert.ToSingle(cmdArgs[2]);
                                break;
                            }
                        default:
                            {
                                SysConsole.WriteLine("Subcommand not recognised.");
                                break;
                            }
                    }

                break;
            }
        case "play":
            {
                if (!GlobalDSP.PlayFile(getRemainderFrom(cmdArgs, 1)))
                    SysConsole.WriteLine("Failed to play file.");
                break;
            }
        case "listen":
            {
                if (cmdArgs.Length < 3)
                    SysConsole.WriteLine("'listen' requires a valid DSP and a path.");
                else
                    switch (cmdArgs[1])
                    {
                        case "TestDSP":
                            {
                                if (!GlobalDSP.ListenFile(getRemainderFrom(cmdArgs, 2), new TestDSP()))
                                    SysConsole.WriteLine("Failed to load file.");
                                break;
                            }
                        default:
                            {
                                SysConsole.WriteLine($"'{cmdArgs[1]}' is not a valid DSP.");
                                break;
                            }
                    }
                
                break;
            }
        case "stop":
            {
                if (!GlobalDSP.Stop())
                    SysConsole.WriteLine("Failed to stop playing.");
                break;
            }
        case "exit":
            {
                IsRunning = false;
                break;
            }
        default:
            {
                SysConsole.WriteLine("Command not recognised.");
                break;
            }
    }
}

#endregion