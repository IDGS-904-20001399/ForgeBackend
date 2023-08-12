namespace Forge.Contracts.Supplies
{
    public record BuySupplyRequest(
        int SupplyId,
        int Quantity
    );
}