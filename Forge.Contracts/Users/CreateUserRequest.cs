namespace Forge.Contracts.Users
{
    public record CreateUserRequest(
        string Email,
        string Password,
        string ConfirmPassword,
        int RoleId
    );
}