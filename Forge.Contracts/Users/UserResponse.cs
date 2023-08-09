namespace Forge.Contracts.Users
{
    public record UserResponse(
        int Id,
        string Email,
        int RoleId
    );
}