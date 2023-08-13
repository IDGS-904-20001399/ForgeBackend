namespace Forge.Contracts.Customers
{
    public record BuyResponse(
        bool Success,
        string Message
    );
}