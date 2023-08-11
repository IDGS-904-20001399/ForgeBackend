namespace Forge.Contracts.Products
{
    public record DetailProductResponse(
        int Id,
        List<DetailSupplyResponse> ProductSupplies,
        List<DetailSupplyResponse> OtherSupplies
    );
}