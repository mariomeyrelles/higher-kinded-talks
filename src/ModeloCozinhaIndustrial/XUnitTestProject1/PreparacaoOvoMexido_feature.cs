using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.MsTest2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ModeloCozinhaIndustrialExemplo2
{

    [TestClass]
    [FeatureDescription(
@"In order to prepare an scrambled egg
As an Chef
I want to use the ingredients to prepare the dish")]
    [Label("Story-1")]
    public partial class PreparacaoOvoMexido_feature
    {
        [Scenario]
        [Label("Scenario-1")]
        public void Prepare_Scrambled_Eggs()
        {
            Runner.RunScenario(
                _ => Given_kitchen_is_available(),
                _ => Given_chef_is_available(),
                _ => When_an_order_arrives_at_the_kitchen("Table1", "Scrambled Egg"),
                _ => Then_dish_preparation_is_started(),
                _ => Then_recipe_is_selected(),
                _ => Then_ingredients_are_selected(),
                _ => Then_chef_prepares_the_dish(),
                _ => Then_order_is_fullfilled()
             );
        }
      
    }
}
