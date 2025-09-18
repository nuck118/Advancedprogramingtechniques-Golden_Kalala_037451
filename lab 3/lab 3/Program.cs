using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab3
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public ICollection<Class> Classes { get; set; } = new List<Class>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} (Id={Id})";
        }
    }

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public override string ToString()
        {
            return $"{Name} (Id={Id})";
        }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public ICollection<Class> Classes { get; set; } = new List<Class>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} (Id={Id})";
        }
    }

    public class MyDatabaseContext : DbContext
    {
        public string DbPath { get; }

        public MyDatabaseContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "lab3.db");
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Classes)
                .WithMany(c => c.Students);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using var db = new MyDatabaseContext();
            Console.WriteLine($"Using database path: {db.DbPath}");

            try
            {
                
                ClearDatabase(db);

                
                var prog = new Class { Name = "Advanced programing techniques" };
                var physics = new Class { Name = "Physics" };

                var golden = new Student { FirstName = "Golden", LastName = "Kalala" };
                var john = new Student { FirstName = "John", LastName = "walker" };

                prog.Students.Add(golden);
                prog.Students.Add(john);
                physics.Students.Add(john);

                db.Classes.AddRange(prog, physics);
                db.Students.AddRange(golden, john);

                db.SaveChanges();
                Console.WriteLine("Added initial classes and students.\n");

                PrintAllStudentsAndClasses(db);

                
                var teacher1 = new Teacher { FirstName = "Jane", LastName = "Newman" };
                db.Teachers.Add(teacher1);
                db.SaveChanges();

                var classToAssign = db.Classes.FirstOrDefault(c => c.Name == "Advanced programing techniques");
                if (classToAssign != null)
                {
                    classToAssign.Teacher = teacher1; 
                    db.SaveChanges();
                    Console.WriteLine($"Assigned teacher {teacher1} to class {classToAssign}.\n");

                    var confirm = db.Classes.Include(c => c.Teacher).FirstOrDefault(c => c.Id == classToAssign.Id);
                    Console.WriteLine($"After assignment: Class '{confirm?.Name}' has teacher: {confirm?.Teacher}\n");
                }

                
                var teacher2 = new Teacher { FirstName = "Anna", LastName = "Zielinska" };
                db.Teachers.Add(teacher2);
                db.SaveChanges();

                var chemistry = new Class { Name = "Chemistry", Teacher = teacher2 };
                db.Classes.Add(chemistry);
                db.SaveChanges();
                Console.WriteLine("Added another teacher and class (Chemistry).\n");

                PrintClasses(db);

                Console.WriteLine($"Removing teacher: {teacher2}");
                var t2 = db.Teachers.FirstOrDefault(t => t.Id == teacher2.Id);
                if (t2 != null)
                {
                    db.Teachers.Remove(t2);
                    db.SaveChanges();
                }

                Console.WriteLine("After removing teacher2:");
                PrintClasses(db);

                
                var remainingClass = db.Classes.FirstOrDefault();
                if (remainingClass != null)
                {
                    Console.WriteLine($"Removing class: {remainingClass}");
                    db.Classes.Remove(remainingClass);
                    db.SaveChanges();
                }

                Console.WriteLine("Teachers after removing a class:");
                PrintTeachers(db);

               
                var teacherWithClasses = db.Teachers.Include(t => t.Classes).FirstOrDefault(t => t.Id == teacher1.Id);
                if (teacherWithClasses != null)
                {
                    Console.WriteLine($"Teacher {teacherWithClasses} has classes:");
                    foreach (var c in teacherWithClasses.Classes)
                    {
                        Console.WriteLine($" - {c}");
                    }
                }
                else
                {
                    Console.WriteLine("Teacher1 not found.");
                }

                Console.WriteLine("\nFinal state of Students and Classes:");
                PrintAllStudentsAndClasses(db);
                Console.WriteLine("\nDone.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving changes: {ex.InnerException?.Message} at {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message} at {ex.StackTrace}");
            }
        }

        static void ClearDatabase(MyDatabaseContext db)
        {
            db.Students.ExecuteDelete();
            db.Classes.ExecuteDelete();
            db.Teachers.ExecuteDelete();
            db.SaveChanges();
            Console.WriteLine("Database cleared (all Students, Classes, Teachers removed).\n");
        }

        static void PrintAllStudentsAndClasses(MyDatabaseContext db)
        {
            var students = db.Students.Include(s => s.Classes).ToList();
            Console.WriteLine("Students:");
            if (students.Count == 0) Console.WriteLine(" (none)");
            foreach (var s in students)
            {
                Console.WriteLine($" - {s}");
                foreach (var c in s.Classes)
                    Console.WriteLine($"    * {c}");
            }

            var classes = db.Classes.Include(c => c.Students).Include(c => c.Teacher).ToList();
            Console.WriteLine("\nClasses:");
            if (classes.Count == 0) Console.WriteLine(" (none)");
            foreach (var c in classes)
            {
                Console.WriteLine($" - {c} Teacher: {(c.Teacher != null ? c.Teacher.ToString() : "none")}");
                foreach (var s in c.Students)
                    Console.WriteLine($"    Student: {s}");
            }
            Console.WriteLine();
        }

        static void PrintClasses(MyDatabaseContext db)
        {
            var classes = db.Classes.Include(c => c.Teacher).ToList();
            Console.WriteLine("Classes:");
            if (!classes.Any()) Console.WriteLine(" (none)");
            foreach (var c in classes)
            {
                Console.WriteLine($" - {c} Teacher: {(c.Teacher != null ? c.Teacher.ToString() : "none")}");
            }
            Console.WriteLine();
        }

        static void PrintTeachers(MyDatabaseContext db)
        {
            var teachers = db.Teachers.ToList();
            Console.WriteLine("Teachers:");
            if (!teachers.Any()) Console.WriteLine(" (none)");
            foreach (var t in teachers)
            {
                Console.WriteLine($" - {t}");
            }
            Console.WriteLine();
        }
    }
}