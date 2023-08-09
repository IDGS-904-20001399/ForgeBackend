namespace Forge.Contracts.Suppliers
{
    public record CreateSupplierRequest(
        string Name,
        string Email,
        string Phone
    );
}