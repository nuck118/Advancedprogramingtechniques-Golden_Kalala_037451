using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class Program
{
    public static void Main()
    {

        int[] numbers = {
            106,104,10,5,117,174,95,61,74,145,77,95,72,59,114,95,61,116,
            106,66,75,85,104,62,76,87,70,17,141,39,199,91,37,139,88,84,15,
            166,118,54,42,123,53,183,95,101,112,26,41,135,70,48,59,69,109,
            93,110,153,178,117,5
        };

        Console.WriteLine("Task 1a: Numbers > 80 (Query syntax)");
        var q1a = from n in numbers where n > 80 select n;
        Console.WriteLine(string.Join(", ", q1a));

        Console.WriteLine("Task 1a: Numbers > 80 (Method syntax)");
        var m1a = numbers.Where(n => n > 80);
        Console.WriteLine(string.Join(", ", m1a));

        Console.WriteLine("\nTask 1b: Order descending (Query)");
        var q1b = from n in numbers orderby n descending select n;
        Console.WriteLine(string.Join(", ", q1b));

        Console.WriteLine("Task 1b: Order descending (Method)");
        var m1b = numbers.OrderByDescending(n => n);
        Console.WriteLine(string.Join(", ", m1b));

        Console.WriteLine("\nTask 1c: Transform into string (Query)");
        var q1c = from n in numbers select $"Have number {n}";
        foreach (var s in q1c) Console.WriteLine(s);

        Console.WriteLine("Task 1c: Transform into string (Method)");
        var m1c = numbers.Select(n => $"Have number {n}");
        foreach (var s in m1c) Console.WriteLine(s);

        Console.WriteLine("\nTask 1d: Count numbers 70 < n < 100");
        var q1d = from n in numbers where n < 100 && n > 70 select n;
        int count1d = q1d.Count();
        Console.WriteLine($"Count = {count1d}");


        City[] cities = {
            new City("Toronto", 100200),
            new City("Hamilton", 80923),
            new City("Ancaster", 4039),
            new City("Brantford", 500890),
        };

        Person[] persons = {
            new Person("Cedric","Coltrane","Toronto",157,null),
            new Person("Hank","Spencer","Peterborough",158,"Sulfa, Penicillin"),
            new Person("Sara","di","29",145,null),
            new Person("Daphne","Seabright","Ancaster",146,null),
            new Person("Rick","Bennett","Ancaster",220,null),
            new Person("Amy","Leela","Hamilton",172,"Penicillin"),
            new Person("Woody","Bashir","Barrie",153,null),
            new Person("Tom","Halliwell","Hamilton",179,"Codeine, Sulfa"),
            new Person("Rachel","Winterbourne","Hamilton",163,null),
            new Person("John","West","Oakville",138,null),
            new Person("Jon","Doggett","Hamilton",194,"Peanut Oil"),
            new Person("Angel","Edwards","Brantford",176,null),
            new Person("Brodie","Beck","Carlisle",157,null),
            new Person("Beanie","Foster","Ancaster",154,"Ragweed, Codeine"),
            new Person("Nino","Andrews","Hamilton",186,null),
            new Person("John","Farley","Hamilton",213,null),
            new Person("Nea","Kobayakawa","Toronto",147,null),
            new Person("Laura","Halliwell","Brantford",146,null),
            new Person("Lucille","Maureen","Hamilton",184,null),
            new Person("Jim","Thoma","Ottawa",173,null),
            new Person("Roderick","Payne","Halifax",58,null),
            new Person("Sam","Threep","Hamilton",199,null),
            new Person("Bertha","Crowley","Delhi",125,"Peanuts, Gluten"),
            new Person("Roland","Edge","Brantford",199,null),
            new Person("Don","Wiggum","Hamilton",189,null),
            new Person("Anthony","Maxwell","Oakville",92,null),
            new Person("James","Sullivan","Delhi",139,null),
            new Person("Anne","Marlowe","Pickering",165,"Peanut Oil"),
            new Person("Kelly","Hamilton","Stoney",84,null),
            new Person("Charles","Andonuts","Hamilton",62,null),
            new Person("Temple","Russert","Hamilton",166,"Sulphur"),
            new Person("Don","Edwards","Hamilton",215,null),
            new Person("Alice","Donovan","Hamilton",167,null),
            new Person("Stone","Cutting","Hamilton",110,null),
            new Person("Neil","Allan","Cambridge",203,null),
            new Person("Cross","Gordon","Ancaster",125,null),
            new Person("Phoebe","Bigelow","Thunder",183,null),
            new Person("Harry","Kuramitsu","Hamilton",210,null)
        };

        Console.WriteLine("\nTask 2a: Persons with height 157 (Query)");
        var q2a = from p in persons where p.Height == 157 select p;
        foreach (var p in q2a) Console.WriteLine(p);

        Console.WriteLine("Task 2a: Persons with height 157 (Method)");
        var m2a = persons.Where(p => p.Height == 157);
        foreach (var p in m2a) Console.WriteLine(p);

        Console.WriteLine("\nTask 2b: Name into 'J. Doe' (Query)");
        var q2b = from p in persons select $"{p.FirstName[0]}. {p.LastName}";
        foreach (var s in q2b) Console.WriteLine(s);

        Console.WriteLine("Task 2b: Name into 'J. Doe' (Method)");
        var m2b = persons.Select(p => $"{p.FirstName[0]}. {p.LastName}");
        foreach (var s in m2b) Console.WriteLine(s);

        Console.WriteLine("\nTask 2c: Distinct allergies (Query)");
        var q2c = (from p in persons
                   where !string.IsNullOrEmpty(p.Allergies)
                   from a in p.Allergies.Split(',')
                   select a.Trim()).Distinct();
        foreach (var a in q2c) Console.WriteLine(a);

        Console.WriteLine("Task 2c: Distinct allergies (Method)");
        var m2c = persons
            .Where(p => !string.IsNullOrEmpty(p.Allergies))
            .SelectMany(p => p.Allergies.Split(','))
            .Select(a => a.Trim())
            .Distinct();
        foreach (var a in m2c) Console.WriteLine(a);

        Console.WriteLine("\nTask 2d: Number of cities starting with H");
        int cityCount = cities.Count(c => c.Name.StartsWith("H"));
        Console.WriteLine(cityCount);

        Console.WriteLine("\nTask 2e: Join persons with cities (pop > 100,000)");
        var q2e = from p in persons
                  join c in cities on p.City equals c.Name
                  where c.Population > 100000
                  select p;
        foreach (var p in q2e) Console.WriteLine(p);

        Console.WriteLine("\nTask 2f: Persons living in or not in given cities");
        var selectedCities = new List<string> { "Toronto", "Hamilton", "Brantford" };
        var inCities = persons.Where(p => selectedCities.Contains(p.City));
        var notInCities = persons.Where(p => !selectedCities.Contains(p.City));

        Console.WriteLine("In cities:");
        foreach (var p in inCities) Console.WriteLine(p);
        Console.WriteLine("Not in cities:");
        foreach (var p in notInCities) Console.WriteLine(p);


        Console.WriteLine("\nTask 3: Persons to XML");
        var xml = new XElement("Persons",
            persons.Select(p =>
                new XElement("Person",
                    new XElement("FirstName", p.FirstName),
                    new XElement("LastName", p.LastName),
                    new XElement("City", p.City),
                    new XElement("Height", p.Height),
                    p.Allergies != null ? new XElement("Allergies", p.Allergies) : null
                )
            )
        );
        Console.WriteLine(xml);
    }
}


public class City
{
    public string Name { get; set; }
    public int Population { get; set; }
    public City(string name, int pop) { Name = name; Population = pop; }
    public override string ToString() => $"{Name} ({Population})";
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

    public override string ToString()
        => $"{FirstName} {LastName}, {City}, {Height}cm, Allergies: {Allergies}";
}
