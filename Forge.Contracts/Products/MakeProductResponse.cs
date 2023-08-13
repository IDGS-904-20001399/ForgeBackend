namespace Forge.Contracts.Products
{
    public record MakeProductResponse(
        string Message,
        List<MissingSupplyResponse> MissingSupplies
    );
}