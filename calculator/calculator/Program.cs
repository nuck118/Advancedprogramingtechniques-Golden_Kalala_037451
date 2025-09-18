// Program.cs — Step 1
using System;
using System.Collections.Generic;

internal class Program
{
    static bool runs = true;

    private static void Main(string[] args)
    {
        var commands = new Dictionary<string, Operation>
        {
            { "sum", new Sum() },
            { "dif", new Dif() },
            { "exit", new Exit(ExitProgram) }
        };

        while (runs)
        {
            string? command = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(command)) continue;

            if (commands.TryGetValue(command, out var op))
            {
                Console.WriteLine(op.Perform());
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }

    private static void ExitProgram()
    {
        throw new NotImplementedException();
    }
}

interface Operation
{
    string Perform();
}

class Sum : Operation
{
    public string Perform()
    {
        int firstTerm = int.Parse(Console.ReadLine() ?? "0");
        int secondTerm = int.Parse(Console.ReadLine() ?? "0");
        return (firstTerm + secondTerm).ToString();
    }
}

class Dif : Operation
{
    public string Perform()
    {
        int firstTerm = int.Parse(Console.ReadLine() ?? "0");
        int secondTerm = int.Parse(Console.ReadLine() ?? "0");
        return (firstTerm - secondTerm).ToString();
    }
}
class Exit : Operation
{
    private readonly Action exitAction;

    public Exit(Action exitAction)
    {
        this.exitAction = exitAction;
    }

    public string Perform()
    {
        exitAction(); // flips runs = false
        return "Program end";
    }
}