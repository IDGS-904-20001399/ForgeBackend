namespace Forge.Contracts.Supplies
{
    public class DetailSupplyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string BuyUnit { get; set; }
        public string UseUnit { get; set; }
        public double Equivalence { get; set; }
        public string Image { get; set; }
        public double Stock { get; set; }
        public double StockInUseUnit { get; set; }
        public string InventoryStatus { get; set; }
        public List<SupplyBuyResponse> Buys {get; set;}
        public List<SupplyBuyResponse> Inventory {get; set;}

        public DetailSupplyResponse()
        {
        }
    }

    public class SupplyBuyResponse
    {
        public int Id { get; set; }
        public DateTime BuyDate { get; set; }
        public int Quantity { get; set; }
        public float AvailableUseQuantity { get; set; }
        public float UnitCost { get; set; }
        public int SupplyId { get; set; }
        public SupplyBuyResponse()
        {
        }
    }
}