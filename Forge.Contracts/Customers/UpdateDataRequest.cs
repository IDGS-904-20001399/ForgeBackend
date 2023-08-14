namespace Forge.Contracts.Customers
{
    public record UpdateDataRequest(
       int UserId,
        string Names,
        string Lastnames,
        string Address,
        string Phone,
        string Email
    );
}