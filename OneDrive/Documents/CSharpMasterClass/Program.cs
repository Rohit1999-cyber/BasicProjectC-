using System;
using System.Net.Http.Headers;
namespace CSharpMasterClass
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (DbConfig.IsUsingFallback)
            {
                Console.WriteLine("Warning: MYSQL_CONN is not set. Using fallback connection string.");
            }

            MySQLCRUD mySQLCRUD = new MySQLCRUD(DbConfig.ConnectionString);
            mySQLCRUD.dbconnect();
            mySQLCRUD.dbinsert("Rohit", "Computer Science", 110, "B", 23);
            mySQLCRUD.dbinsert("Vaibhav", "Mechanical Engineering", 103, "B", 21);
            mySQLCRUD.GetStudents();
            mySQLCRUD.UpdateStudentAge(110, 21);
            mySQLCRUD.GetStudents();
            mySQLCRUD.DeleteStudent(102);
            mySQLCRUD.GetStudents();
            MySQLDemo mySQLDemo = new MySQLDemo(DbConfig.ConnectionString);
            mySQLDemo.connect();
            // Prompt the user and read a line of text
            // Console.Write("Enter a string: ");
            // string word = Console.ReadLine() ?? string.Empty;
            // Console.WriteLine("Title Case: " + word.ToTitleCase());
            // Console.ReadKey();
            // Animal a = new Dog();
            // a.MakeSound();
            // a.Sleep();
            // Animal b = new Cat();
            // b.MakeSound();
            // b.Sleep();
        }
    }
}