using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    /*
     * The Pluralsight tutorial said that it's generally best to have one class per file. The video tutorials, however,
     * included multiple classes in a single file for viewing simplicity.
     */

    // This NamedObject class is intended to demonstrate inheritance.
    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get; set;
        }
    }

    public abstract class Book: NamedObject, IBook
    {
        public Book(string name): base(name)
        {

        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade); // implicitly virtual

        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {

        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            // In case an exception is thrown, this "using" statement guarantees writer.Dispose().
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs()); 
                }
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            using (var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }

            return result;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public class InMemoryBook: Book, IBook
    {
        //private List<double> grades;

        /*
        public string Name
        {
            get; 
            set;
        }
        */
        public List<double> Grades
        {
            get;
            set;
        }

        public const string CATEGORY = "Science";

        public InMemoryBook(string name) : base(name)
        {
            Grades = new List<double>();
            Name = name;
        }

        public override void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                Grades.Add(grade);
                if (GradeAdded != null)
                {
                    // event from the delegate
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        // This method was only meant to demonstrate overloading and switch statements. It does not get used.
        public void AddGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        // Calculate the statistics
        public override Statistics GetStatistics()
        {
            var result = new Statistics();


            for (var index = 0; index < Grades.Count; index += 1)
            {
                result.Add(Grades[index]);
            }

            return result;
        }
    }
}
