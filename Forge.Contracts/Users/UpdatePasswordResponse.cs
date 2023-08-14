namespace Forge.Contracts.Users
{
    public record UpdatePasswordResponse(
        bool Success,
        string Message
    );
}