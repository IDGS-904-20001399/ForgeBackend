
using System.Data.Common;
using ErrorOr;
using Forge.Models;
using Forge.Persistence;
using Forge.ServiceErrors;
using Microsoft.EntityFrameworkCore;

namespace Forge.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ForgeDbContext _dbContext;

        public ProductService(ForgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ErrorOr<Created> CreateProduct(Product product)
        {
            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return Result.Created;
        }



        public ErrorOr<Deleted> DeleteProduct(Guid id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return Errors.Product.NotFound;
            }

            _dbContext.Remove(product);
            _dbContext.SaveChanges();

            return Result.Deleted;
        }

        public ErrorOr<Product> GetProduct(Guid id)
        {
            if (_dbContext.Products.Find(id) is Product product)
            {
                return product;
            }
            return Errors.Product.NotFound;
        }

        public ErrorOr<UpsertedProduct> UpsertProduct(Product product)
        {
            bool isNewlyCreated = !_dbContext.Products.Any(p => p.Id == product.Id);

            if (isNewlyCreated)
            {
                _dbContext.Add(product);
            }
            else
            {

                _dbContext.Update(product);
            }
            _dbContext.SaveChanges();

            return new UpsertedProduct(isNewlyCreated);
        }
    }

}