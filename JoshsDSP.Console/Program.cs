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

        if (cmdArgs.Count <= argCounter) cmdArgs.Add(arg);
        else cmdArgs[argCounter] += $" {arg}";

        if (!isInQuotes) argCounter++;
    }

    return cmdArgs.ToArray();
}

/// <summary>
/// Parses a command string and returns the first argument
/// </summary>
string getFirstArg(string cmd) => cmd.Split(' ')[0];

/// <summary>
/// Parses a command string and returns the remainder of the string, staring from an argument index.
/// </summary>
string getLastArgFrom(string cmd, int index)
{
    string[] args = getArgs(cmd);
    string[] lastArgs = new string[args.Length - index];

    for (int i = 0; i < lastArgs.Length; i++) lastArgs[i] = args[index + i];

    return string.Join(' ', lastArgs);
}

#endregion

#region Main

bool IsRunning = true;
string? cmd;

while (IsRunning)
{
    SysConsole.Write(">");
    cmd = SysConsole.ReadLine();

    //Ignore command if null (should never be true) or if it is empty
    if (cmd is null || cmd == string.Empty) continue;

    string firstArg = getFirstArg(cmd).ToLower(); //commands are not case sensitive

    //TBI (eventually): an actual command system; this current one is really basic
    switch (firstArg)
    {
        case "play":
            {
                if (!GlobalDSP.PlayFile(getLastArgFrom(cmd, 1)))
                    SysConsole.WriteLine("Failed to play file.");
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