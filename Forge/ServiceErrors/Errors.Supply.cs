using ErrorOr;

namespace Forge.ServiceErrors
{
    public static partial class Errors
    {
        public static class Supply{
            public static Error InvalidName => Error.Validation(
                code: "Supply.InvalidName",
                description: $"The name of the product must be at least {Models.Product.MinFieldLength} characters long"
            );

            public static Error InvalidPrice => Error.Validation(
                code: "Supply.InvalidPrice",
                description: $"The price must be greater or equal to {Models.Product.MinPrice}"
            );
            public static Error NotFound => Error.NotFound(
                code: "Supply.NotFound",
                description: "Supply not found"
            );
        }
    }
}