namespace Forge.Contracts.Dashboard
{
    public class ProductSold
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int UnitsSold { get; set; }
        public float Earnings { get; set; }

        public int TotalUnitsSold{get; set;}
        public float TotalEarnings{get; set;}

        public ProductSold() { }
    }
}