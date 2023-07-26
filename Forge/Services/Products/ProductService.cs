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
                parameters.Add("Id", product.Id);
                parameters.Add("Name", product.Name);
                parameters.Add("Description", product.Description);
                parameters.Add("Category", product.Category);
                parameters.Add("Price", product.Price);
                parameters.Add("Image", product.Image);
                parameters.Add("is_inserted", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Execute the stored procedure using Dapper
                _dbConnection.Execute("upsert_product", parameters, commandType: CommandType.StoredProcedure);
                int isNewRecordCreated = parameters.Get<int>("is_inserted");
                Debug.WriteLine("--------------------------------444-----------------------------------------");
                Debug.WriteLine(isNewRecordCreated);

                // Get the value of the output parameter to check if a new rec
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