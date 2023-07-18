using ErrorOr;

namespace Forge.ServiceErrors
{
    public static class Errors
    {
        public static class Product{

            public static Error InvalidName => Error.Validation(
                code: "Product.InvalidName",
                description: $"The name of the product must be at least {Models.Product.MinFieldLength} characters long"
            );

            public static Error InvalidPrice => Error.Validation(
                code: "Product.InvalidPrice",
                description: $"The price must be greater or equal to {Models.Product.MinPrice}"
            );
            public static Error NotFound => Error.NotFound(
                code: "Product.NotFound",
                description: "Product not found"
            );
        }
    }
}