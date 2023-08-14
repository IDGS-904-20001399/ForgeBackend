namespace Forge.Contracts.Users
{
    public record UpdateEmailRequest(
        int UserId,
        string Email
    );
}