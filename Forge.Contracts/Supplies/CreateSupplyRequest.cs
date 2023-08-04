namespace Forge.Contracts.Supplies
{
    public record CreateSupplyRequest(
        string Name,
        double Cost,
        string BuyUnit,
        string UseUnit,
        double Equivalence,
        string Image
    );
}