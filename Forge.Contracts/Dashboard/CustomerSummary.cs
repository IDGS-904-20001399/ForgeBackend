namespace Forge.Contracts.Dashboard
{
    public class CustomerSummary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int TotalQuantity{get; set;}
        public float TotalPrice { get; set; }

        public CustomerSummary()
        {

        }
    }
}