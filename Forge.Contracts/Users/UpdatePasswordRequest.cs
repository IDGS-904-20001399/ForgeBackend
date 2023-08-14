namespace Forge.Contracts.Users
{
    public record UpdatePasswordRequest(
        int UserId,
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword
    );
}