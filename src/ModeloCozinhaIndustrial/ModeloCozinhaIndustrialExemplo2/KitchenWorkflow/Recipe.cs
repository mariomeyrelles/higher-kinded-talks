using System.Collections.Generic;
using System.Linq;

namespace ModeloCozinhaIndustrialExemplo2.KitchenWorkflow
{
    public class Recipe
    {
        public Recipe(string recipeName, List<Ingredient> ingredients, List<RecipeStep> steps)
        {
            this.RecipeName = recipeName;
            this.Ingredients = ingredients;
            this.Steps = steps;
          
        }

        public string RecipeName { get; }
        public IReadOnlyList<RecipeStep> Steps { get; }
        public IReadOnlyList<Ingredient> Ingredients { get;  }

        internal RecipeStep GetFirstStep()
        {
            return this.Steps.First();
        }
    }
}
