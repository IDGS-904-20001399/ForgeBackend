using ErrorOr;

namespace Forge.ServiceErrors
{
    public static class Errors
    {
        public static class Product{
            public static Error NotFound => Error.NotFound(
                code: "Product.NotFound",
                description: "Product not found"
            );
        }
    }
}