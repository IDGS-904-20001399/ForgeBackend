namespace Forge.Contracts.Suppliers
{
    public record SupplierResponse(
        int Id,
        string Name,
        string Email,
        string Phone
    );
}