using System;
using System.Configuration;
namespace AddressBook
{
    class Program
    {

        /* I love coding */
        static void Main(string[] args)
        {
            string name = ConfigurationManager.AppSettings["ApplicationName"];
            string connectionString = ConfigurationManager.ConnectionStrings["AddressBook"].ConnectionString;

            Console.WriteLine("WELCOME TO:");
            Console.WriteLine(name);
            Console.WriteLine(new string('_', Console.WindowWidth - 4));
            Console.WriteLine();
            Console.WriteLine("Press enter to Continue");
            Console.ReadLine();
            Rolodex rolodex = new Rolodex(connectionString);
            rolodex.DoStuff();
        }
    }
}
