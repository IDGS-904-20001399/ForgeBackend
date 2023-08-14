using ErrorOr;

namespace Forge.ServiceErrors
{
    public static partial class Errors
    {
        public static class User{
            public static Error EmailTaken => Error.Validation(
                code: "User.EmailTaken",
                description: $"El correo proporcionado ya se encuentra en uso"
            );

        }
    }
}