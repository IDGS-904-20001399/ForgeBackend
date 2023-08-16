namespace Forge.Contracts.Dashboard
{
    public class ProductInventory
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int? AvailableQuantity { get; set; }
        public string Image { get; set; }

        public ProductInventory()
        {

        }
    }
}