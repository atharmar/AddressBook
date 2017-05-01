using System.Collections.Generic;

namespace AddressBook
{
    public interface IHandleRecipes
    {
        int Count { get; }

        void Create(string title, RecipeType choice);
        List<Recipe> GetAllRecipes();
    }
}