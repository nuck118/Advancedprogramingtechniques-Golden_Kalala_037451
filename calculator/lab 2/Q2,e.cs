using System;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var cities = Data.GetCities();
        var persons = Data.GetPersons();

        var query = from p in persons
                    join c in cities on p.City equals c.Name
                    where c.Population > 100000
                    select p;

        Console.WriteLine("Persons from cities with population > 100,000:");
        foreach (var p in query) Console.WriteLine(p);
    }
}

public class City
{
    public string Name;
    public int Population;
    public City(string name, int pop) { Name = name; Population = pop; }
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
    public static City[] GetCities() => new City[]
    {
        new City("Toronto", 100200),
        new City("Hamilton", 80923),
        new City("Ancaster", 4039),
        new City("Brantford", 500890),
    };

    public static Person[] GetPersons() => new Person[]
    {
        new Person("Cedric","Coltrane","Toronto",157,null),
        new Person("Angel","Edwards","Brantford",176,null),
        new Person("Amy","Leela","Hamilton",172,"Penicillin"),
    };
}
