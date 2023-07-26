using ErrorOr;
using Forge.Contracts.Products;
using Forge.Models;

namespace Forge.Services.Products
{
    public interface IProductService
    {
        ErrorOr<Created> CreateProduct(Product product);
        ErrorOr<Deleted> DeleteProduct(Guid id);
        ErrorOr<Product> GetProduct(Guid id);
        ErrorOr<UpsertedProduct> UpsertProduct(Product product);
        // ProductResponse GetProduct(Guid id);
        // ProductResponse UpdateProduct(Guid id, UpsertProductRequest request);
        // ProductResponse DeleteProduct(Guid id);
    }
}