namespace Forge.Contracts.Users
{
    public record UserResponse(
        int Id,
        string Email,
        string Password,
        string ConfirmPassword,
        int RoleId
    );
}