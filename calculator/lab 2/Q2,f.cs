using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        var persons = Data.GetPersons();
        var selectedCities = new List<string> { "Toronto", "Hamilton", "Brantford" };

        var inCities = persons.Where(p => selectedCities.Contains(p.City));
        var notInCities = persons.Where(p => !selectedCities.Contains(p.City));

        Console.WriteLine("Persons in selected cities:");
        foreach (var p in inCities) Console.WriteLine(p);

        Console.WriteLine("\nPersons not in selected cities:");
        foreach (var p in notInCities) Console.WriteLine(p);
    }
}

public class Person
{
    public string FirstName, LastName, City;
    public int Height;
    public string? Allergies;
    public Person(string fn, string ln, string city, int h, string? allergies)
    { FirstName = fn; LastName = ln; City = city; Height = h; Allergies = allergies; }
    public override string ToString() => $"{FirstName} {LastName}, {City}";
}

public static class Data
{
    public static Person[] GetPersons() => new Person[]
    {
        new Person("Cedric","Coltrane","Toronto",157,null),
        new Person("Amy","Leela","Hamilton",172,"Penicillin"),
        new Person("Woody","Bashir","Barrie",153,null),
    };
}
