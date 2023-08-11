namespace Forge.Contracts.Products
{
    public class DetailSupplyResponse
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Cost { get; private set; }
        public string BuyUnit { get; private set; }
        public string UseUnit { get; private set; }
        public double Equivalence { get; private set; }
        public string Image { get; private set; }
        public double Quantity { get; private set; }

        public DetailSupplyResponse() { }
    }
}
