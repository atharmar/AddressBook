using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace AddressBook
{
    public class Rolodex
    {
        public Rolodex(string connectionString)
        {
            _connectionString = connectionString;
            _contacts = new List<Contact>();
            _recipes = new Dictionary<RecipeType, List<Recipe>>();
            _recipes.Add(RecipeType.Appetizers, new List<Recipe>()); // Add if there is not one or it throw exception           
            _recipes[RecipeType.Entrees] = new List<Recipe>();  //override whats out there and can use "COntainsKey check"
            _recipes.Add(RecipeType.Desserts, new List<Recipe>());         
           
        }

        public void DoStuff()
        {
            // Print a menu
            ShowMenu();
            // Get the user's choice
            MenuOption choice = GetMenuOption();
            
            // while the user does not want to exit
            while (choice != MenuOption.Exit)
            {
                // figure out what they want to do
                // get information
                // do stuff
                switch(choice)
                {
                    case MenuOption.AddPerson:
                        DoAddPerson();
                        break;
                    case MenuOption.AddCompany:
                        DoAddCompany();
                        break;
                    case MenuOption.ListContacts:
                        DoListContacts();
                        break;
                    case MenuOption.SearchContacts:
                        DoSearchContacts();
                        break;
                    case MenuOption.RemoveContact:
                        DoRemoveContact();
                        break;
                    case MenuOption.AddRecipe:
                        DoAddRecipe();
                        break;
                    case MenuOption.ListRecipes:
                        DoListRecipes();
                        break;
                    case MenuOption.SearchEverything:
                        DoSearchEverything();
                        break;
                }
                ShowMenu();
                choice = GetMenuOption();
            }
        }

        private void DoAddRecipe()
        {
            Console.Clear();
            Console.WriteLine("Please enter your recipe title");
            string title = GetNonEmptyStringFromUser();
            Recipe recipe = new Recipe(title);
            Console.WriteLine("What kind of recipe is this?");
            for (int i =0; i < (int)RecipeType.UPPER_LIMIT; i += 1)
            {
                Console.WriteLine($"{i}.{(RecipeType)i}");
            }
            /* string input = Console.ReadLine();
            int num  = int.Parse(input);
            RecipeType  = (RecipeType) num; 
            one line code below replace all the above 3 */
            RecipeType choice = (RecipeType) int.Parse(Console.ReadLine());
            List<Recipe> specificRecipes = _recipes[choice];
            specificRecipes.Add(recipe);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                insert into recipe(RecipeType, Recipe)
                values (@Type, @name)
                ";
                command.Parameters.AddWithValue("@Type", choice);
                command.Parameters.AddWithValue("@name", title);
                command.ExecuteNonQuery();
            }
        }


        private void DoSearchEverything()
        {
            Console.Clear();
            Console.WriteLine("SEARCH!");
            Console.Write("Please enter a search term: ");
            string term = GetNonEmptyStringFromUser();
            List<IMatchATerm> matchables = new List<IMatchATerm>();
            matchables.AddRange(_contacts);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Recipe TypeId, Recipe Type, Recipe from Recipe";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string recipeTitle = reader.GetString(1);
                    Recipe recipe = new Recipe(recipeTitle);
                    matchables.Add(recipe);
                }
                foreach (IMatchATerm matcher in matchables)
                {
                    if (matcher.Matches(term))
                    {
                        Console.WriteLine($">{matcher}");
                    }
                }

                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        private void DoRemoveContact()
        {
            Console.Clear();
            Console.WriteLine("REMOVE A CONTACT!");
            Console.Write("Search for a contact: ");
            string term = GetNonEmptyStringFromUser();

            foreach (Contact contact in _contacts)
            {
                if (contact.Matches(term))
                {
                    Console.Write($"Remove {contact}? (y/N)"); // hit 'enter' lines to 'N'//
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        _contacts.Remove(contact);
                        return;
                    }
                }
            }

            Console.WriteLine("No more contacts found.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
        }

        private void DoSearchContacts()
        {
            Console.Clear();
            Console.WriteLine("SEARCH!");
            Console.Write("Please enter a search term: ");
            string term = GetNonEmptyStringFromUser();

            foreach (Contact contact in _contacts)
            {
                if (contact.Matches(term))
                {
                    Console.WriteLine($"> {contact}");
                }
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private void DoListContacts()
        {
            Console.Clear();
            Console.WriteLine("YOUR CONTACTS");
            //SortedList<string, Contact> contacts = new SortedList<string, Contact>();
            string fullPath = GetFolderPath();
            using (StreamReader reader = File.OpenText(fullPath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split('|');
                     if (parts[0] == "C")
                    {
                        Console.WriteLine(parts[1], parts[2]);
                    }
                     else if (parts[0] == "P")
                    {
                        Console.WriteLine(parts[1], parts[2], parts[3]);
                    }
                     else
                    {
                        Console.WriteLine("You have jumk in your contacts file");
                    }
                }
            }

            foreach (Contact contact in _contacts)
            {
                Console.WriteLine($"> {contact}");
            }

            Console.ReadLine();
            Console.ReadLine();
        }

        private void DoListRecipes()
        {
            Console.Clear();
            Console.WriteLine("RECIPES");
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Recipe TypeId, Recipe Type, Recipe FROM Recipe";
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int rowId = reader.GetInt32(0);
                    string Type = reader.GetString(1);
                    string Recipe = reader.GetString(2);
                    Console.WriteLine($"{rowId},{Type},{Recipe}");
                }              
            }
            Console.ReadLine();
        }


        private string GetFolderPath ()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = "CONTACTS.DAT";
            string fullPath = Path.Combine(desktopPath, fileName);
            return fullPath;
        }
        private void DoAddCompany()
        {
            Console.Clear();
            Console.WriteLine("Please enter information about the company.");
            Console.Write("Company name: ");
            string name = Console.ReadLine();
            Console.Write("Phone number: ");
            string phoneNumber = GetNonEmptyStringFromUser();
            // write to a file
            string fullPath = GetFolderPath();
            using (StreamWriter writer = new StreamWriter(fullPath, true)) //relative path            
            {
                writer.WriteLine($"C|,{name},|,{phoneNumber}");             
            }

 //           _contacts.Add(new Company(name, phoneNumber));
        }

        private void DoAddPerson()
        {
            Console.Clear();
            Console.WriteLine("Please enter information about the person.");
            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = GetNonEmptyStringFromUser();

            Console.Write("Phone number: ");
            string phoneNumber = GetNonEmptyStringFromUser();
            // Write to a file 
            string fullPath = GetFolderPath();
            using (StreamWriter writer = new StreamWriter(fullPath, true))//relative path            
            {
                writer.WriteLine(string.Join("|",firstName , lastName,phoneNumber));
            }

         //   _contacts.Add(new Person(firstName, lastName, phoneNumber));
        }

        private string GetNonEmptyStringFromUser()
        {
            string input = Console.ReadLine();
            while (input.Length == 0)
            {
                Console.WriteLine("That is not valid.");
                input = Console.ReadLine();
            }
            return input;
        }

        private int GetNumberFromUser()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    return int.Parse(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine("you should type a number");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("THAT WAS BAD! DO AGAIN");
                }
                catch (Exception)
                {
                    Console.WriteLine("Capture any kind of exception");
                }
                finally
                {
                    Console.WriteLine("This will always be printed");

                }
            }
        }

        private MenuOption GetMenuOption()
        {

            int choice = GetNumberFromUser();

            while (choice < 0 || choice > (int)MenuOption.UPPER_LIMIT)
            {
                Console.WriteLine("That is not valid.");
                choice = GetNumberFromUser();
            }

            return (MenuOption)choice;
        }

        private void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine($"ROLODEX! ({_contacts.Count}) ({_recipes.Count})");
            Console.WriteLine("1. Add a person");
            Console.WriteLine("2. Add a company");
            Console.WriteLine("3. List all contacts");
            Console.WriteLine("4. Search contacts");
            Console.WriteLine("5. Remove a contact");
            Console.WriteLine("------------------------");
            Console.WriteLine("6. Add Recipe");
            Console.WriteLine("7. List all Recipes");
            Console.WriteLine("------------------------");
            Console.WriteLine("8. Search Everything");
            Console.WriteLine();
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("What would you like to do? ");
        }    
      

        private readonly List<Contact> _contacts;
        private Dictionary<RecipeType, List<Recipe>> _recipes;
        private readonly string _connectionString;
    }
}
