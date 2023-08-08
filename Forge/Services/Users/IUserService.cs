using ErrorOr;
using Forge.Models;

namespace Forge.Services.Users
{
    public interface IUserService
    {
        ErrorOr<Created> CreateUser(User user);
        ErrorOr<Deleted> DeleteUser(int id);
        ErrorOr<User> GetUser(int id);
        ErrorOr<List<User>> GetUsers();
        ErrorOr<UpsertedRecord> UpsertUser(User user);
    }
}