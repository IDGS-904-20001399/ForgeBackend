using System.Data;
using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;
using Forge.Contracts.Products;

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
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(product);
                parameters.Add("InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("add_product", parameters, commandType: CommandType.StoredProcedure);
                product.Id = parameters.Get<int>("InsertedId");
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
                string query = "UPDATE product SET STATUS = 0 WHERE id = @Id";
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
                string query = "SELECT * FROM product WHERE id = @Id AND status = 1";
                Product product = _dbConnection.QueryFirstOrDefault<Product>(query, new { Id = id });
                if (product != null)
                {
                    return product;
                }
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
                string query = "SELECT * FROM product WHERE status = 1";
                return _dbConnection.Query<Product>(query).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Product.NotFound;

        }

        public ErrorOr<Created> AddSupplies(DetailProductRequest request)
        {
            // Delete existing rows
            string query = "DELETE FROM product_supplies WHERE product_id = @Id";
            _dbConnection.Execute(query, new { Id = request.Id });

            string supplyQuery = "INSERT INTO product_supplies (product_id, supply_id, quantity)" +
                                "VALUES (@ProductID, @SupplyID, @Quantity)";
            foreach (var supply in request.Supplies)
            {
                _dbConnection.Execute(supplyQuery, new { ProductID = request.Id, SupplyID = supply.SupplyId, Quantity = supply.Quantity });
            }

            return Result.Created;
        }


        public ErrorOr<UpsertedProduct> UpsertProduct(Product product)
        {
            bool isNewlyCreated = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(product);
                parameters.Add("Created", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("upsert_product", parameters, commandType: CommandType.StoredProcedure);
                isNewlyCreated = parameters.Get<int>("Created") == 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return new UpsertedProduct(isNewlyCreated);
        }

        public ErrorOr<DetailProductResponse> GetProductDetails(int id)
        {
            string productSuppliesQuery = "SELECT s.*, ps.quantity FROM supply s INNER JOIN product_supplies ps ON s.id = ps.supply_id WHERE ps.product_id = @Id";
            List<DetailSupplyResponse> productSupplies = _dbConnection.Query<DetailSupplyResponse>(productSuppliesQuery, new { Id = id }).ToList();

            string otherSuppliesQuery = "SELECT s.* from supply s where s.id not in (select supply_id from product_supplies where product_id = @Id)";
            List<DetailSupplyResponse> otherSupplies = _dbConnection.Query<DetailSupplyResponse>(otherSuppliesQuery, new { Id = id }).ToList();


            return new DetailProductResponse(id, productSupplies, otherSupplies);
        }
    }

}