using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var persons = Data.GetPersons();

        var query = from p in persons
                    where p.Height == 157
                    select p;

        var method = persons.Where(p => p.Height == 157);

        Console.WriteLine("Query syntax result:");
        foreach (var p in query) Console.WriteLine(p);

        Console.WriteLine("\nMethod syntax result:");
        foreach (var p in method) Console.WriteLine(p);
    }
}

public class City
{
    public string Name { get; set; }
    public int Population { get; set; }
    public City(string name, int pop) { Name = name; Population = pop; }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public int Height { get; set; }
    public string? Allergies { get; set; }

    public Person(string fn, string ln, string city, int h, string? allergies)
    {
        FirstName = fn; LastName = ln; City = city; Height = h; Allergies = allergies;
    }

    public override string ToString() => $"{FirstName} {LastName}, {City}, {Height}cm";
}

public static class Data
{
    public static Person[] GetPersons() => new Person[]
    {
        new Person("Cedric","Coltrane","Toronto",157,null),
        new Person("Hank","Spencer","Peterborough",158,"Sulfa, Penicillin"),
        new Person("Sara","di","29",145,null),
        new Person("Daphne","Seabright","Ancaster",146,null),
        new Person("Rick","Bennett","Ancaster",220,null),
    };
}
