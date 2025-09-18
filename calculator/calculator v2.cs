using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        // Use standard .NET delegates Action<T> and Func<T> for better interoperability.
        Action<string> output = Console.WriteLine;
        Func<int> input = ReadInteger;

        // The command pattern is great! We can make it more robust and reusable.
        var commands = new Dictionary<string, IOperation>
        {
            // By creating a generic BinaryOperation class, we avoid code duplication.
            { "sum", new BinaryOperation(output, input, (a, b) => a + b, "Sum") },
            { "subtract", new BinaryOperation(output, input, (a, b) => a - b, "Difference") },
            { "multiply", new BinaryOperation(output, input, (a, b) => a * b, "Product") },
            { "divide", new BinaryOperation(output, input, (a, b) => a / b, "Quotient") },
            { "exit", new Exit(output) }
        };

        bool continueRunning = true;
        while (continueRunning)
        {
            output("\nEnter a command (sum, subtract, multiply, divide, exit):");
            string? command = Console.ReadLine();

            // Use TryGetValue for a safer and more efficient dictionary lookup.
            if (command != null && commands.TryGetValue(command, out var operation))
            {
                // The operation itself now controls whether the loop should continue.
                // This avoids using a global static flag like 'runs'.
                continueRunning = operation.Perform();
            }
            else
            {
                output("Unknown command");
            }
        }
    }

    /// <summary>
    /// A robust method to read an integer from the console, handling invalid input.
    /// </summary>
    private static int ReadInteger()
    {
        while (true)
        {
            string? line = Console.ReadLine();
            if (int.TryParse(line, out int value))
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
}

// It's a common C# convention to prefix interface names with 'I'.
interface IOperation
{
    /// <summary>
    /// Performs the operation.
    /// </summary>
    /// <returns>true to continue the program, false to exit.</returns>
    bool Perform();
}

// This single class replaces the duplicated 'Sum' and 'Dif' classes.
class BinaryOperation : IOperation
{
    private readonly Action<string> _output;
    private readonly Func<int> _input;
    private readonly Func<int, int, int> _calculation;
    private readonly string _resultName;

    public BinaryOperation(Action<string> output, Func<int> input, Func<int, int, int> calculation, string resultName)
    {
        _output = output;
        _input = input;
        _calculation = calculation;
        _resultName = resultName;
    }

    public bool Perform()
    {
        _output("Enter the first number:");
        int a = _input();
        _output("Enter the second number:");
        int b = _input();
        int result = _calculation(a, b);
        _output($"{_resultName}: {result}");
        return true; // Signal to continue running.
    }
}

class Exit : IOperation
{
    private readonly Action<string> _output;

    public Exit(Action<string> output)
    {
        _output = output;
    }

    public bool Perform()
    {
        _output("Program end");
        return false; // Signal to stop running.
    }
}
