namespace Forge.Contracts.Dashboard
{
    public class ProductSummary
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week1 { get; set; }
        public int Week2 { get; set; }
        public int Week3 { get; set; }
        public int Week4 { get; set; }
        public int TotalCreated { get; set; }

        public ProductSummary()
        {
        }
    }
}