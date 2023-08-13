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
                string query = "SELECT p.*, (SELECT SUM(available_quantity) from product_inventory WHERE product_id = p.id) stock FROM `product` p WHERE status = 1 and p.id = @Id";
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
                string query = "SELECT p.*, (SELECT SUM(available_quantity) from product_inventory WHERE product_id = p.id) stock FROM `product` p WHERE status = 1;";
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

        public ErrorOr<MakeProductResponse> MakeProduct(MakeProductRequest request)
        {
            // Check that there are enough supplies to make the product
            List<MissingSupplyResponse> MissingSupplies = new();
            string message = "Not enough supplies";

            string query = "CALL product_supply_details(@Id)";
            var productSupplies = _dbConnection.Query<dynamic>(query, new { Id = request.ProductId });
            double productionCost = 0;
            foreach (var productSupply in productSupplies)
            {
                var quantityNeeded = productSupply.quantity * request.Quantity;
                var missingQuantity = quantityNeeded - productSupply.stock_in_use_unit;
                productionCost += productSupply.quantity_cost;
                if (missingQuantity > 0)
                {
                    var buyMissingQuantity = Math.Ceiling(missingQuantity / productSupply.equivalence);
                    MissingSupplies.Add(
                        new MissingSupplyResponse(
                            productSupply.name,
                            missingQuantity,
                            productSupply.use_unit,
                            buyMissingQuantity,
                            productSupply.buy_unit
                            )
                    );
                }
            }

            // Make the product. Add row to the inventory
            if (MissingSupplies.Count == 0)
            {
                var InventoryParameters = new
                {
                    CreationDate = DateTime.Today,
                    Quantity = request.Quantity,
                    AvailableQuantity = request.Quantity,
                    UnitCost = productionCost,
                    ProductId = request.ProductId
                };

                _dbConnection.Execute("InsertProductInventory", InventoryParameters, commandType: CommandType.StoredProcedure);

                // Decrease the used supplies from warehouse
                foreach (var productSupply in productSupplies)
                {
                    var spent_quantity = productSupply.quantity * request.Quantity;
                    var buysQuery = "SELECT * from supply_buys WHERE supply_id = @Id order by buy_date ASC";
                    var buys = _dbConnection.Query(buysQuery, new { Id = productSupply.id }).ToArray();
                    int index = 0;
                    var remaining = spent_quantity;
                    string updateSuppliesQuery = "";

                    while (true)
                    {
                        var buy = buys[index];
                        var difference = buy.available_use_quantity - remaining;
                        if (difference >= 0)
                        {
                            updateSuppliesQuery = "UPDATE supply_buys SET available_use_quantity = @NewValue WHERE id = @Id";
                            _dbConnection.Execute(updateSuppliesQuery, new { Id = buy.id, NewValue = difference });
                            break;
                        }
                        remaining = Math.Abs(difference);
                        difference = 0;
                        updateSuppliesQuery = "UPDATE supply_buys SET available_use_quantity = @NewValue WHERE id = @Id";
                        _dbConnection.Execute(updateSuppliesQuery, new { Id = buy.id, NewValue = 0 });
                        index++;
                    }
                }

                message = "Product was made successfully";

            }


            return new MakeProductResponse(message, MissingSupplies);
        }
    }

}