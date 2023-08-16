namespace Forge.Contracts.Dashboard
{
    public class MonthlyProductSold
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalProductsSold { get; set; }

        public MonthlyProductSold()
        {
        }
    }
}