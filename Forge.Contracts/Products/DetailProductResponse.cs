namespace Forge.Contracts.Products
{
    public record DetailProductResponse(
        int Id,
        List<DetailSupplyResponse> ProductSupplies,
        List<DetailSupplyResponse> OtherSupplies,
        List<InventoryResponse> Inventory,
        List<InventoryResponse> Productions
    );
}