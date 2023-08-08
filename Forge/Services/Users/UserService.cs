using ErrorOr;
using Forge.Models;
using Forge.Services;

namespace Forge.Services.Users
{
    public class UserService : IUserService
    {
        public ErrorOr<Created> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public ErrorOr<Deleted> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public ErrorOr<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public ErrorOr<List<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public ErrorOr<UpsertedRecord> UpsertUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}