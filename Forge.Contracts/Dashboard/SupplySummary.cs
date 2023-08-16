public class SupplySummary
{
    public int Id { get; set; }
    public int SupplyId { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int QuantityBought { get; set; }
    public float Percentage { get; set; }

    public SupplySummary()
    {
    }
}
