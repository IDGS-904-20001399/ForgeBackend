using System.Data;
using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;

namespace Forge.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly MySqlConnection _dbConnection;

        public ProductService(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public ErrorOr<Created> CreateProduct(Product product)
        {
            try
            {
                string query = "CALL add_product(@Name, @Description, @Category, @Price, @Image)";
                product.Id = _dbConnection.QueryFirstOrDefault<int>(query, product);
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Result.Created;
        }

        public ErrorOr<Deleted> DeleteProduct(int id)
        {
            try
            {
                string query = "DELETE FROM product WHERE id = @Id";
                _dbConnection.Execute(query, new { Id = id });
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            // _products.Remove(id);
            return Result.Deleted;
        }

        public ErrorOr<Product> GetProduct(int id)
        {
            try
            {
                string query = "SELECT * FROM product WHERE id = @Id";
                return _dbConnection.QueryFirstOrDefault<Product>(query, new { Id = id });
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Product.NotFound;
        }

        public ErrorOr<List<Product>> GetProducts()
        {
            try
            {
                string query = "SELECT * FROM product";
                return _dbConnection.Query<Product>(query).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Product.NotFound;

        }

        public ErrorOr<UpsertedProduct> UpsertProduct(Product product)
        {
            bool isNewlyCreated = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(product);
                parameters.Add("is_inserted", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("upsert_prodcut", parameters, commandType: CommandType.StoredProcedure);
                isNewlyCreated = parameters.Get<int>("is_inserted") == 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return new UpsertedProduct(isNewlyCreated);
        }
    }

}