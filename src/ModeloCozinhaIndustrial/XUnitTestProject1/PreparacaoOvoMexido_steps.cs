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
        private DishPreparationOrder currentOrder;
        private string currentChef;
        private OrderPreparationRequest currentOrderPreparationRequest;

        private void Given_kitchen_is_available()
        {
            kitchen = new Kitchen();
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

        private object Then_recipe_is_selected()
        {
            throw new NotImplementedException();
        }

        private object Then_ingredients_are_selected()
        {
            throw new NotImplementedException();
        }

        private object Then_chef_prepares_the_dish()
        {
            throw new NotImplementedException();
        }


        private void Then_order_is_fullfilled()
        {

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
