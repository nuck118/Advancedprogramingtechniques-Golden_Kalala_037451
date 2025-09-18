using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var persons = Data.GetPersons();

        var query = (from p in persons
                     where !string.IsNullOrEmpty(p.Allergies)
                     from a in p.Allergies.Split(',')
                     select a.Trim()).Distinct();

        var method = persons
            .Where(p => !string.IsNullOrEmpty(p.Allergies))
            .SelectMany(p => p.Allergies.Split(','))
            .Select(a => a.Trim())
            .Distinct();

        Console.WriteLine("Query syntax result:");
        foreach (var a in query) Console.WriteLine(a);

        Console.WriteLine("\nMethod syntax result:");
        foreach (var a in method) Console.WriteLine(a);
    }
}

public class Person
{
    public string FirstName, LastName, City;
    public int Height;
    public string? Allergies;
    public Person(string fn, string ln, string city, int h, string? allergies)
    { FirstName = fn; LastName = ln; City = city; Height = h; Allergies = allergies; }
}

public static class Data
{
    public static Person[] GetPersons() => new Person[]
    {
        new Person("Hank","Spencer","Peterborough",158,"Sulfa, Penicillin"),
        new Person("Amy","Leela","Hamilton",172,"Penicillin"),
        new Person("Tom","Halliwell","Hamilton",179,"Codeine, Sulfa"),
    };
}
