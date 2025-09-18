using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        int[] numbers = {
            106,104,10,5,117,174,95,61,74,145,77,95,72,59,114,95,61,116,106,66,75,85,104,62,76,87,70,17,141,39,199,91,37,139,88,84,15,
            166,118,54,42,123,53,183,95,101,112,26,41,135,70,48,59,69,109,93,110,153,178,117,5 };

        var query = from n in numbers
                    orderby n descending
                    select n;

        var method = numbers.OrderByDescending(n => n);

        Console.WriteLine("Query syntax result:");
        Console.WriteLine(string.Join(", ", query));

        Console.WriteLine("\nMethod syntax result:");
        Console.WriteLine(string.Join(", ", method));
    }
}
