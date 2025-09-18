using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        int[] numbers = {
            106,104,10,5,117,174,95,61,74,145,77,95,72,59,114,95,61,116,106,66,75,85,104,62,76,87,70,17,141,39,199,91,37,139,88,84,15,
            166,118,54,42,123,53,183,95,101,112,26,41,135,70,48,59,69,109,93,110,153,178,117,5};

        var query = from n in numbers
                    where n < 100 && n > 70
                    select n;

        int count = query.Count();

        Console.WriteLine("Numbers between 70 and 100 (exclusive):");
        Console.WriteLine(string.Join(", ", query));

        Console.WriteLine($"\nCount = {count}");
    }
}
