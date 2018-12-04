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

            var scrambledEggIngredients = new List<Ingredient>
            {
                new Ingredient(name: "Ovo", quantity: 1.0m, uom: "unidade"),
                new Ingredient(name: "Leite", quantity: 1.0m, uom: "xícara"),
                new Ingredient(name: "Manteiga", quantity: 2.0m, uom: "colher"),
                new Ingredient(name: "Sal", quantity: 1.0m, uom: "pitada")
            };

            var scrambledEggPreparationSteps = new List<RecipeStep>
            {
                new RecipeStep(description: "Coloque os ovos em recipiente e deixe por 2 minutos"),
                new RecipeStep(description: "Ligue a frigideira, coloque a manteiga e mexa bem"),
                new RecipeStep(description: "Coloque os ovos e mexa bem"),
                new RecipeStep(description: "Junte o leite e continue mexendo"),
                new RecipeStep(description: "Coloque o sal e mexa até ficar consistente"),
                new RecipeStep(description: "Finalização", isLastStep: true)
            };

            var scrambledEgg = new Recipe(recipeName: "Ovo Mexido", ingredients: scrambledEggIngredients, steps: scrambledEggPreparationSteps);
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
            Assert.IsTrue(orderPreparationRequest.PreparationRequestStatus == orderPreparationRequest.PreparationRequestStatus);
            currentOrderPreparationRequest = orderPreparationRequest;

        }

        private void Then_recipe_is_selected()
        {
            var recipe = kitchen.GetRecipe(currentOrderPreparationRequest);
            currentRecipe = recipe;
            currentOrder.Recipe = recipe;
        
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
            Assert.IsTrue(currentRecipe.Steps[4].IsLastStep == false);
            Assert.IsTrue(currentRecipe.Steps[5].Description == "Finalização");
            Assert.IsTrue(currentRecipe.Steps[5].IsLastStep == true);
        }


        private void Then_chef_prepares_the_dish()
        {

            var firstStep = currentOrderPreparationRequest.StartExecution();
            var secondStep = currentOrderPreparationRequest.CompleteStep(firstStep);
            var thirdStep = currentOrderPreparationRequest.CompleteStep(secondStep);
            var fourthStep = currentOrderPreparationRequest.CompleteStep(thirdStep);
            var fifthStep = currentOrderPreparationRequest.CompleteStep(fourthStep);
            var completionStep = currentOrderPreparationRequest.CompleteStep(fifthStep);
            var afterCompletionStep = currentOrderPreparationRequest.CompleteStep(completionStep);

            Assert.IsTrue(currentOrderPreparationRequest.PreparationRequestStatus == PreparationRequest.Completed);
            Assert.IsTrue(afterCompletionStep.IsLastStep == true);
            Assert.IsTrue(afterCompletionStep.CompletedAt > DateTime.MinValue);
            Assert.IsTrue(afterCompletionStep.CompletedAt <= currentOrder.PreparationEndTime);




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

        public DateTime PreparationStartTime { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public DateTime PreparationEndTime { get; internal set; }
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

    

    public class RecipeStep
    {

        public RecipeStep(string description, bool isLastStep = false)
        {
            this.Description = description;
            this.IsLastStep = isLastStep;
        }

        public bool IsLastStep { get; }
        public string Description { get; }
        public DateTime StartedAt { get; protected set; }
        public DateTime CompletedAt { get; protected set; }

        public void SetStartTime()
        {
            this.StartedAt = DateTime.Now;
        }

        public void SetCompleteTime()
        {
            this.CompletedAt = DateTime.Now;
        }
    }




    internal class OrderPreparationRequest
    {
        public DishPreparationOrder Order { get; internal set; }
        public PreparationRequest PreparationRequestStatus { get; internal set; }

        public RecipeStep StartExecution()
        {
            this.PreparationRequestStatus = PreparationRequest.WorkInProgress;
            this.Order.PreparationStartTime = DateTime.Now;
            var firstStep = this.Order.Recipe.GetFirstStep();
            return firstStep;
        }

        public RecipeStep CompleteStep(RecipeStep step)
        {
            var steps = this.Order.Recipe.Steps.ToList();
            step.SetCompleteTime();

            var nextStepId = steps.IndexOf(step) + 1;
            var nextStep = step;

            if (nextStepId <= steps.Count -1)
            {
                nextStep = steps[nextStepId];
                nextStep.SetStartTime();
            }
            else
            {
                this.Order.PreparationEndTime = DateTime.Now;
                this.PreparationRequestStatus = PreparationRequest.Completed;
            }

            
            return nextStep;
        }

    }

    enum PreparationRequest
    {
        Accepted = 1,
        Rejected = 2,
        WorkInProgress = 3,
        Completed = 4
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
