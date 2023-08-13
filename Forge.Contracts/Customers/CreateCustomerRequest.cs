namespace Forge.Contracts.Customers
{
    public record CreateCustomerRequest(
        string Email,
        string Password,
        string ConfirmPassword,
        string Names,
        string Lastnames,
        string Address,
        string Phone
    );
}