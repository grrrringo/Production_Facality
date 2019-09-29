using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_Facility.Models
{
    [Table("Recipe")]
    public class Recipe
    {
        [Key]
        public int RecipeID { get; set; }
        public string RecipeOwner { get; set; }

        public string RecipeComposition { get; set; }

        public List<RecipeLine> ItemRecipe = new List<RecipeLine>();

        

        public Recipe(string owner)
        {
            this.RecipeOwner = owner;
        }
        public Recipe()
        {

        }

        public RecipeLine recipeLine;

        public class RecipeLine
        {
            public int RecipeLine_Nr { get; set; }
            public string RecipeLine_Key { get; set; }
            public string RecipeLine_Name { get; set; }
            public double RecipeLine_Amount { get; set; }

            public RecipeLine(int line_nr, string key, string name, double amount)
            {
                this.RecipeLine_Nr = line_nr;
                this.RecipeLine_Key = key;
                this.RecipeLine_Name = name;
                this.RecipeLine_Amount = amount;
            }
        }
    }
}
