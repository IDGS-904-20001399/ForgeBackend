using ErrorOr;
using Forge.Contracts.Users;

namespace Forge.Models
{
    public class User
    {
        public int Id { get; set;}
        public string Email { get; private set;}
        public string Password { get; private set;}
        public int Active  { get; private set;}
        public string ConfirmedAt  { get; private set;}
        public int Status  { get; private set;}
        public int RoleId  { get; private set;}
        public string Role  { get; private set;}
        public User(){}

        private User(string email, string password, int roleId){
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        public static ErrorOr<User> From(CreateUserRequest request){
            return Create(
                request.Email,
                request.Password,
                request.RoleId
            );
        }

        public static ErrorOr<User> From(int id, CreateUserRequest request){
            return Create(
                request.Email,
                request.Password,
                request.RoleId,
                id
            );
        }

        public static ErrorOr<User> Create(string email, string password, int roleId, int? id = 0){
            return new User(email, password, roleId){Id = id ?? 0};
        }

    }
}