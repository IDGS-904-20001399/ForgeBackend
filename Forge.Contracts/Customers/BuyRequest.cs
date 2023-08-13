namespace Forge.Contracts.Customers
{
    public record BuyRequest(
        int CustomerId,
        CardRequest Card,
        List<BuyItemRequest> Items
    );
}