namespace Forge.Contracts.Users
{
    public record LoginResponse(
        bool Authenticated,
        string Token,
        string Message,
        string Name, 
        string Role,
        int Id
    );
}