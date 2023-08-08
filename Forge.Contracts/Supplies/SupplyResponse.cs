namespace Forge.Contracts.Supplies
{
    public record SupplyResponse(
        int Id,
        string Name,
        double Cost,
        string BuyUnit,
        string UseUnit,
        double Equivalence,
        string Image
    );
}