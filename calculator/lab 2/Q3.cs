using System;
using System.Linq;
using System.Xml.Linq;

public class Program
{
    public static void Main()
    {
        var persons = Data.GetPersons();


        var xml = new XElement("Persons",
            persons.Select(p =>
                new XElement("Person",
                    new XElement("FirstName", p.FirstName),
                    new XElement("LastName", p.LastName),
                    new XElement("City", p.City),
                    new XElement("Height", p.Height),
                    p.Allergies != null
                        ? new XElement("Allergies", p.Allergies)
                        : null
                )
            )
        );


        Console.WriteLine(xml);
    }
}

public class Person
{
    public string FirstName, LastName, City;
    public int Height;
    public string? Allergies;

    public Person(string fn, string ln, string city, int h, string? allergies)
    {
        FirstName = fn; LastName = ln; City = city; Height = h; Allergies = allergies;
    }

    public override string ToString()
        => $"{FirstName} {LastName}, {City}, {Height}cm, Allergies: {Allergies}";
}

public static class Data
{
    public static Person[] GetPersons() => new Person[]
    {
        new Person("Cedric","Coltrane","Toronto",157,null),
        new Person("Hank","Spencer","Peterborough",158,"Sulfa, Penicillin"),
        new Person("Amy","Leela","Hamilton",172,"Penicillin"),
        new Person("Tom","Halliwell","Hamilton",179,"Codeine, Sulfa"),
        new Person("Angel","Edwards","Brantford",176,null),
    };
}
