namespace Forge.Contracts.Users
{
    public record UpsertUserRequest(
    string Email,
    int RoleId
);
}