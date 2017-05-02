using Dapper.Contrib.Extensions;

namespace AddressBook
{
    public class Recipe : IMatchable
    {
        public Recipe() {} // using empty construtor for dapper//
        public Recipe(string title, RecipeType type)
        {
            _title = title;
            _type = type;
        }

        [Key]
        public int RecipeID { get; set; }

        public string Name
        {
            get { return _title; } // string someTitle = recipe Title;
            set { _title = value; }//recipe.Title = "Peanut Brittle?, value <- "peanut Brittle"
        }

        public RecipeType RecipeTypeId
        {
            get { return _type; }  
            set { _type = value; } 
        }


        public bool Matches(string term)
        {
            return _title.ToLower().Contains(term.ToLower());
        }

        public override string ToString()
        {
            return $"RECIPE: {_title}";
        }

        private string _title;
        private RecipeType _type;
    }
}
