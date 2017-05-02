using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AddressBook
{
    public class RecipesRepository : IHandleRecipes
    {
        private string _connectionString;

        public int Count
        {
            get
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT COUNT(*) FROM Recipes";
                    return (int)command.ExecuteScalar();
                }
            }

        }

        public RecipesRepository(string connectionString)
        {
          _connectionString = connectionString;
        }
        public List<Recipe> GetAllRecipes()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                IEnumerable<Recipe> recipes = connection.GetAll<Recipe>();
                return new List<Recipe>(recipes);             
            }
        }

        public void Create(string title, RecipeType choice)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Insert(new Recipe(title, choice));
            }
        }
    }
}