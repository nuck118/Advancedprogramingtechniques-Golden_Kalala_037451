using System;
using System.Collections.Generic;

internal class Program
{
    static bool runs = true;

    // Delegates
    public delegate void OnResult(string result);
    public delegate int OnInput();

    private static void Main(string[] args)
    {
        OnResult output = Console.WriteLine;
        OnInput input = () => int.Parse(Console.ReadLine());

        var commands = new Dictionary<string, Operation>
        {
            { "sum", new Sum(output, input) },
            { "dif", new Dif(output, input) },
            { "exit", new Exit(() => runs = false, output) }
        };

        while (runs)
        {
            string? command = Console.ReadLine();

            if (commands.ContainsKey(command))
            {
                commands[command].Perform();
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
}

interface Operation
{
    void Perform();
}

class Sum : Operation
{
    private readonly Program.OnResult print;
    private readonly Program.OnInput read;

    public Sum(Program.OnResult print, Program.OnInput read)
    {
        this.print = print;
        this.read = read;
    }

    public void Perform()
    {
        int a = read();
        int b = read();
        print((a + b).ToString());
    }
}

class Dif : Operation
{
    private readonly Program.OnResult print;
    private readonly Program.OnInput read;

    public Dif(Program.OnResult print, Program.OnInput read)
    {
        this.print = print;
        this.read = read;
    }

    public void Perform()
    {
        int a = read();
        int b = read();
        print((a - b).ToString());
    }
}

class Exit : Operation
{
    private readonly Action exitAction;
    private readonly Program.OnResult print;

    public Exit(Action exitAction, Program.OnResult print)
    {
        this.exitAction = exitAction;
        this.print = print;
    }

    public void Perform()
    {
        exitAction();
        print("Program end");
    }
}
