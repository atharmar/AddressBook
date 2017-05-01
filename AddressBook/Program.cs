using System;
using System.Configuration;

namespace AddressBook
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString;
            connectionString = ConfigurationManager.ConnectionStrings["AddressBook"].ConnectionString;

            string contactsFileName = ConfigurationManager.AppSettings["ContactsDatabaseFileName"];

            string name = ConfigurationManager.AppSettings["ApplicationName"];
            Console.WriteLine("WELCOME TO:");
            Console.WriteLine(name);
            Console.WriteLine(new string('-', Console.WindowWidth - 4));
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();

            ContactsRepository contactsRepo = new ContactsRepository(contactsFileName);
            RecipesRepository recipesRepo = new RecipesRepository(connectionString);
            Rolodex rolodex = new Rolodex(contactsRepo, recipesRepo);
            rolodex.DoStuff();
        }
    }
}
