using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var cities = Data.GetCities();

        int count = cities.Count(c => c.Name.StartsWith("H"));

        Console.WriteLine($"Number of cities starting with H: {count}");
    }
}

public class City
{
    public string Name;
    public int Population;
    public City(string name, int pop) { Name = name; Population = pop; }
}

public static class Data
{
    public static City[] GetCities() => new City[]
    {
        new City("Toronto", 100200),
        new City("Hamilton", 80923),
        new City("Halifax", 60000),
        new City("Brantford", 500890),
    };
}
