using System.Collections.Generic;

namespace AddressBook
{
    public interface IHandleRecipes
    {
        void Create(string title, RecipeType choice);
        List<Recipe> GetAllRecipes();
    }
}