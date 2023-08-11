namespace Forge.Contracts.Products
{
    public record DetailSupplyRequest(
        int SupplyId,
        double Quantity
    );
}