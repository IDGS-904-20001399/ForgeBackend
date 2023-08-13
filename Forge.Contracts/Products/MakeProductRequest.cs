namespace Forge.Contracts.Products
{
    public record MakeProductRequest(
        int ProductId,
        int Quantity
    );
}