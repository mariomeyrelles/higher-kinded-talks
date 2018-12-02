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

            scrambledEgg.Ingredients.Add(new Ingredient() { Name = "Ovo",      Quantity = 1.0m, UnitOfMeasure = "unidade" });
            scrambledEgg.Ingredients.Add(new Ingredient() { Name = "Leite",    Quantity = 1.0m, UnitOfMeasure = "xícara" });
            scrambledEgg.Ingredients.Add(new Ingredient() { Name = "Manteiga", Quantity = 2.0m, UnitOfMeasure = "colher" });
            scrambledEgg.Ingredients.Add(new Ingredient() { Name = "Sal",      Quantity = 1.0m, UnitOfMeasure = "pitada" });

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
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient() { Name = "Ovo", Quantity = 1.0m, UnitOfMeasure = "unidade" }));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient() { Name = "Leite", Quantity = 1.0m, UnitOfMeasure = "xícara" }));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient() { Name = "Manteiga", Quantity = 2.0m, UnitOfMeasure = "colher" }));
            Assert.IsTrue(currentRecipe.Ingredients.Contains(new Ingredient() { Name = "Sal", Quantity = 1.0m, UnitOfMeasure = "pitada" }));


        }


        private void Then_recipe_steps_are_available()
        {

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
       
        public string Name { get; internal set; }
        public decimal Quantity { get; internal set; }
        public string UnitOfMeasure { get; internal set; }
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
        public string RecipeName { get; internal set; }
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
