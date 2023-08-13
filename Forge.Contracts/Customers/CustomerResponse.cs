namespace Forge.Contracts.Customers
{
    public record CustomerResponse(
        int Id,
        string Email,
        string Names,
        string Lastnames,
        string Address,
        string Phone
    );
}