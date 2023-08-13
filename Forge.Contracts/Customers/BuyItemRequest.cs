namespace Forge.Contracts.Customers
{
    public record BuyItemRequest(
        int ProductId,
        int Quantity
    );
}