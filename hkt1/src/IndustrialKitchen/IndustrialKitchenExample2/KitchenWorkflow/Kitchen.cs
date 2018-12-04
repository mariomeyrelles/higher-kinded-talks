using System.Linq;
using System.Collections.Generic;

namespace ModeloCozinhaIndustrialExemplo2.KitchenWorkflow
{
    public class Kitchen
    {
        private List<Recipe> recipes = new List<Recipe>();

        internal ChefAvailabilityResult GetChefAvailability()
        {
            var result = new ChefAvailabilityResult
            {
                Availability = ChefAvailability.Idle,
                ChefName = "Chef 1"
            };

            return result;
        }

        internal OrderPreparationRequest StartPreparation(DishPreparationOrder order)
        {
            var request = new OrderPreparationRequest
            {
                Order = order
            };

            return request;
        }

        internal Recipe GetRecipe(OrderPreparationRequest request)
        {
            var recipe = (from x in this.recipes
                         where x.RecipeName == request.Order.Dish
                         select x).Single();
            return recipe;
        }

        internal void RegisterRecipe(Recipe recipe)
        {
            this.recipes.Add(recipe);
        }
    }
}
