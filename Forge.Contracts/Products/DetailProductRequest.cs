namespace Forge.Contracts.Products
{
    public record DetailProductRequest(
        int Id,
        List<DetailSupplyRequest> Supplies
    );
}