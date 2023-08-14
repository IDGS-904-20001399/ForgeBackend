namespace Forge.Contracts.Products
{
    public class InventoryResponse
    {

        public DateTime CreationDate { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public double UnitCost { get; set; }
        public double TotalCost { get; set; }
        public InventoryResponse()
        {
        }
    }
}