using Forge.Contracts.Products;
using Forge.Models;

namespace Forge.Services.Products
{
    public interface IProductService
    {
        void CreateProduct(Product product);
        Product GetProduct(Guid id);
        // ProductResponse GetProduct(Guid id);
        // ProductResponse UpdateProduct(Guid id, UpsertProductRequest request);
        // ProductResponse DeleteProduct(Guid id);
    }
}