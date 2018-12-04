using LightBDD.MsTest2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using IndustrialKitchenExample2.KitchenWorkflow;

namespace IndustrialKitchenExample2
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
            Assert.IsTrue(currentOrder.PreparationStartTime < currentOrder.PreparationEndTime);
        }
    }
}
