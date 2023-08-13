namespace Forge.Contracts.Products
{
    public record MissingSupplyResponse(
        string Name,
        double MissingQuantity,
        string UseUnit,
        double BuyMissingQuantity,
        string BuyUnit
    );
}