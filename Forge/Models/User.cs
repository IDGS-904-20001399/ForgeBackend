using ErrorOr;

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
        public User(){}

        private User(string email, string password){
            Email = email;
            Password = password;
        }

        public static ErrorOr<User> Create(string email, string password, int? id = 0){
            return new User(email, password){Id = id ?? 0};
        }

    }
}