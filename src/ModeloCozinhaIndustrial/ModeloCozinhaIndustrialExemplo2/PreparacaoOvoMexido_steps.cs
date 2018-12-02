using LightBDD.Framework;
using LightBDD.Framework.Formatting;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.MsTest2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ModeloCozinhaIndustrialExemplo2
{

    
    public partial class PreparacaoOvoMexido_feature : FeatureFixture
    {

        private Kitchen kitchen;
        private DishPreparationOrder currentOrder;
        private string currentChef;
        private OrderPreparationRequest currentOrderPreparationRequest;
        private Recipe currentRecipe;


        
        private void Given_kitchen_is_available()
        {
            kitchen = new Kitchen();


            var scrambledEgg = new Recipe();

            scrambledEgg.Ingredients.Add(new Ingredient(name: "Ovo",      quantity: 1.0m, uom: "unidade"));
            scrambledEgg.Ingredients.Add(new Ingredient(name: "Leite",    quantity: 1.0m, uom: "xícara"));
            scrambledEgg.Ingredients.Add(new Ingredient(name: "Manteiga", quantity: 2.0m, uom: "colher"));
            scrambledEgg.Ingredients.Add(new Ingredient(name: "Sal",      quantity: 1.0m, uom: "pitada"));

            scrambledEgg.RecipeName = "Scrambled Egg";


            kitchen.RegisterRecipe(scrambledEgg);
        }

        private void Given_chef_is_available()
        {
            var availabiltyResult = kitchen.GetChefAvailability();

            Assert.IsTrue(availabiltyResult.Availability == ChefAvailability.Idle);

            currentChef = availabiltyResult.ChefName;
        }

        private void When_an_order_arrives_at_the_kitchen(string table, string dish)
        {
            var order = new DishPreparationOrder()
            {
                Table = table,
                Dish = dish,
                ChefName = currentChef
            };

            currentOrder = order;
        }

        private void Then_dish_preparation_is_started()
        {
            var orderPreparationRequest = kitchen.StartPreparation(currentOrder);
            Assert.IsTrue(orderPreparationRequest.Accepted == orderPreparationRequest.Accepted);
            currentOrderPreparationRequest = orderPreparationRequest;

        }

        private void Then_recipe_is_selected()
        {
            var recipe = kitchen.GetRecipe(currentOrderPreparationRequest);
            currentRecipe = recipe;
        
        }

        private void Then_ingredients_are_available()
        {
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient(name: "Ovo",      quantity: 1.0m, uom: "unidade")));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient(name: "Leite",    quantity: 1.0m, uom: "xícara")));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient(name: "Manteiga", quantity: 2.0m, uom: "colher")));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient(name: "Sal",      quantity: 1.0m, uom: "pitada")));


        }


        private void Then_recipe_steps_are_available()
        {
            Assert.IsTrue(currentRecipe.Steps[0].Description == "Coloque os ovos em recipiente e deixe por 2 minutos");
            Assert.IsTrue(currentRecipe.Steps[1].Description == "Ligue a frigideira, coloque a manteiga e mexa bem");
            Assert.IsTrue(currentRecipe.Steps[2].Description == "Coloque os ovos e mexa bem");
            Assert.IsTrue(currentRecipe.Steps[3].Description == "Junte o leite e continue mexendo");
            Assert.IsTrue(currentRecipe.Steps[4].Description == "Coloque o sal e mexa até ficar consistente");
        }


        private object Then_chef_prepares_the_dish()
        {
            throw new NotImplementedException();



        }


        private void Then_order_is_fullfilled()
        {

        }
    }

    public class Ingredient
    {
        public Ingredient(string name, decimal quantity, string uom)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.UnitOfMeasure = uom;

        }

        public string Name { get; }
        public decimal Quantity { get; }
        public string UnitOfMeasure { get; }

        public override bool Equals(object obj)
        {
            var other = (Ingredient)obj;

            if (this.Name == other.Name)
                if (this.Quantity == other.Quantity)
                    if (this.UnitOfMeasure == other.UnitOfMeasure)
                        return true;

            return false;

        }

        public override int GetHashCode()
        {
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode

            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Name.GetHashCode();
                hash = hash * 23 + this.Quantity.GetHashCode();
                hash = hash * 23 + this.UnitOfMeasure.GetHashCode();
                return hash;
            }
        }
    }


    internal class DishPreparationOrder
    {
        public string Table;
        public string Dish;
        public string ChefName;
    }

    internal class Kitchen
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

    internal class Recipe
    {

        public List<Ingredient> Ingredients = new List<Ingredient>();
        public List<RecipeStep> Steps = new List<RecipeStep>();
        public string RecipeName { get; internal set; }
    }

    public class RecipeStep
    {
        internal readonly string Description;
    }

    internal class OrderPreparationRequest
    {
        public DishPreparationOrder Order { get; internal set; }
        public PreparationRequest Accepted { get; internal set; }
    }

    enum PreparationRequest
    {
        Accepted = 1,
        Rejected = 2,
    }

    enum ChefAvailability
    {
        Idle = 1,
        Unavailable = 2,
        AvailableSoon = 3
    }

    class ChefAvailabilityResult
    {
        public ChefAvailability Availability { get; internal set; }
        public string ChefName { get; internal set; }
    }
}
