using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var persons = Data.GetPersons();

        var query = from p in persons
                    select $"{p.FirstName[0]}. {p.LastName}";

        var method = persons.Select(p => $"{p.FirstName[0]}. {p.LastName}");

        Console.WriteLine("Query syntax result:");
        foreach (var s in query) Console.WriteLine(s);

        Console.WriteLine("\nMethod syntax result:");
        foreach (var s in method) Console.WriteLine(s);
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
        new Person("Cedric","Coltrane","Toronto",157,null),
        new Person("Hank","Spencer","Peterborough",158,"Sulfa, Penicillin"),
        new Person("Sara","di","29",145,null),
    };
}
