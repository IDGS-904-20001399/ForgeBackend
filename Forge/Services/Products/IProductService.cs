using Forge.Contracts.Products;
using Forge.Models;

namespace Forge.Services.Products
{
    public interface IProductService
    {
        void CreateProduct(Product product);
        void DeleteProduct(Guid id);
        Product GetProduct(Guid id);
        void UpsertProduct(Product product);
        // ProductResponse GetProduct(Guid id);
        // ProductResponse UpdateProduct(Guid id, UpsertProductRequest request);
        // ProductResponse DeleteProduct(Guid id);
    }
}