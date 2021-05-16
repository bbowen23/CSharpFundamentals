using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        static void Main(string[] args)
        {
            // var book = new InMemoryBook("Benjamin Bowen's Gradebook");
            var book = new DiskBook("Benjamin Bowen's Gradebook");
            book.GradeAdded += OnGradeAdded; // listen to the event contained in Book.cs

            EnterGrades(book);

            var stats = book.GetStatistics();

            Console.WriteLine($"For the book named {book.Name}");
            Console.WriteLine($"Lowest grade: {stats.Low}");
            Console.WriteLine($"Highest grade: {stats.High}");
            Console.WriteLine($"Average grade: {stats.Average:N1}");
            Console.WriteLine($"Letter grade: {stats.Letter}");
        }

        private static void EnterGrades(Book book) // can take any Book type, not just an InMemoryBook
        {
            while (true)
            {
                Console.WriteLine("Enter a grade from 0-100. Alternatively, press 'q' to quit.");
                var input = Console.ReadLine();

                if (input == "q")
                {
                    break;
                }

                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void OnGradeAdded(object sender, EventArgs e)
        {
            Console.WriteLine("A grade was added.");
        }
    }
}
