using ErrorOr;
using Forge.Contracts.Users;

namespace Forge.Services.Login
{
    public interface ILoginService
    {
         ErrorOr<LoginResponse> Login(LoginRequest request);
    }
}