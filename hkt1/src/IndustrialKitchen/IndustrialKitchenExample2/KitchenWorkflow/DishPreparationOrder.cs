using System;

namespace ModeloCozinhaIndustrialExemplo2.KitchenWorkflow
{
    public class DishPreparationOrder
    {
        public string Table;
        public string Dish;
        public string ChefName;

        public DateTime PreparationStartTime { get; internal set; }
        public Recipe Recipe { get; internal set; }
        public DateTime PreparationEndTime { get; internal set; }
    }
}
