namespace IndustrialKitchenExample2.KitchenWorkflow
{
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
}
