using LightBDD.Framework;
using LightBDD.Framework.Formatting;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.MsTest2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ModeloCozinhaIndustrialExemplo2
{

    
    public partial class PreparacaoOvoMexido_feature : FeatureFixture
    {

        private Kitchen kitchen; 

        private void Given_kitchen_is_available()
        {
            kitchen = new Kitchen();
        }
        private void Given_chef_is_available()
        {
            var availabiltyResult = kitchen.GetChefAvailability();

            Assert.IsTrue(availabiltyResult.Availability == ChefAvailability.Idle);
           
        }

        private void When_an_order_arrives_at_the_kitchen(string table, string order)
        {

        }

        private void Then_dish_must_be_prepared()
        {

        }


    }

    internal class Kitchen
    {
        internal ChefAvailabilityResult GetChefAvailability()
        {
            throw new NotImplementedException();
        }
    }

    enum ChefAvailability
    {
        Idle = 1,
        Unavailable = 2,
        AvailableSoon = 3
    }

    class ChefAvailabilityResult
    {
        public ChefAvailability Availability;
        public string ChefName;
    }
}
