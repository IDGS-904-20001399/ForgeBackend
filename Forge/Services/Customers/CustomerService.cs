using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;
using Forge.Contracts.Products;
using Forge.Contracts.Customers;
using System.Data;
using System.Reflection;

namespace Forge.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly MySqlConnection _dbConnection;

        public CustomerService(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public ErrorOr<BuyResponse> Buy(BuyRequest request)
        {
            List<Dictionary<string, object>> products = new();

            double subtotal = 0;
            bool enoughStock = true;
            string Message = "Order created successfully";


            foreach (var item in request.Items)
            {
                string query = "SELECT p.*, (SELECT SUM(available_quantity) from product_inventory WHERE product_id = p.id) stock FROM `product` p WHERE status = 1 and p.id = @Id";
                Product product = _dbConnection.QueryFirstOrDefault<Product>(query, new { Id = item.ProductId });
                if (product.Stock < item.Quantity)
                {
                    enoughStock = false;
                    break;
                }
                subtotal += product.Price * item.Quantity;
                var dict = ObjectToDictionary(product);
                dict.Add("Quantity", item.Quantity);
                products.Add(dict);
            }

            if (enoughStock)
            {
                double DeliveryFee = subtotal >= 1000 ? 0 : 99;
                double total = subtotal + DeliveryFee;

                var parameters = new DynamicParameters();
                parameters.Add("InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("Payment", $"Card {MaskString(request.Card.CardNumber)}");
                parameters.Add("DeliveryFee", DeliveryFee);
                parameters.Add("Status", "Created");
                parameters.Add("DateValue", DateTime.Now);
                parameters.Add("UserId", request.CustomerId);

                string insertQuery = "InsertOrder";
                _dbConnection.Execute(insertQuery,
                                     parameters,
                                     commandType: CommandType.StoredProcedure);

                int insertedId = parameters.Get<int>("InsertedId");

                foreach (var product in products)
                {
                    var DetailsParameters = new
                    {
                        OrderId = insertedId,
                        ProductId = product["Id"],
                        Quantity = product["Quantity"],
                        Price = product["Price"]
                    };

                    // Call the stored procedure using Dapper
                    _dbConnection.Execute("InsertOrderDetail", DetailsParameters, commandType: CommandType.StoredProcedure);

                    string inventoryQuery = "SELECT * FROM product_inventory WHERE product_id = @Id";

                    var inventoryRows = _dbConnection.Query<dynamic>(inventoryQuery, new { Id = product["Id"] }).ToArray();
                    int index = 0;
                    int remaining = (int)product["Quantity"];
                    string updateQuery = "";

                    while (true)
                    {
                        var inventory = inventoryRows[index];
                        int difference = inventory.available_quantity - remaining;
                        if (difference >= 0)
                        {
                            updateQuery = "UPDATE product_inventory SET available_quantity = @NewValue WHERE id = @Id";
                            _dbConnection.Execute(updateQuery, new { NewValue = difference, Id = inventory.id });
                            break;
                        }
                        remaining = Math.Abs(difference);
                        difference = 0;
                        updateQuery = "UPDATE product_inventory SET available_quantity = @NewValue WHERE id = @Id";
                        _dbConnection.Execute(updateQuery, new { NewValue = 0, Id = inventory.id });
                        index += 1;
                    }
                }
            }
            else
            {
                Message = "Not enough stock";
            }


            return new BuyResponse(enoughStock, Message);
        }

        static string MaskString(string input)
        {
            int length = input.Length;

            if (length <= 4)
            {
                return input;
            }

            int maskedLength = length - 4;
            string maskedPart = new string('*', maskedLength);
            string lastFourDigits = input.Substring(maskedLength);

            return maskedPart + lastFourDigits;
        }

        public static Dictionary<string, object> ObjectToDictionary(object obj)
        {
            var dictionary = new Dictionary<string, object>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                dictionary[property.Name] = property.GetValue(obj);
            }

            return dictionary;
        }
        public ErrorOr<Created> CreateCustomer(Customer customer)
        {
            try
            {

                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(customer);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("add_user", parameters, commandType: CommandType.StoredProcedure);
                customer.UserId = parameters.Get<int>("Id");

                DynamicParameters CustomerParameters = new DynamicParameters();
                CustomerParameters.AddDynamicParams(customer);
                CustomerParameters.Add("InsertedId", DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("InsertCustomer", CustomerParameters, commandType: CommandType.StoredProcedure);

                customer.Id = CustomerParameters.Get<int>("InsertedId");

            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Result.Created;
        }

        public ErrorOr<List<OrdersResponse>> GetOrders(OrderRequest request)
        {
            string orderQuery = "SELECT o.*, (SELECT SUM(quantity * price) FROM order_details WHERE order_id = o.id) subtotal, (SELECT subtotal + delivery_fee) total FROM `order` o WHERE user_id = @Id;";
            var Orders = _dbConnection.Query<OrdersResponse>(orderQuery, new {Id = request.CustomerId}).ToList();
            foreach(var order in Orders){
                string detailsQuery = "SELECT od.*, (SELECT name from product where id = od.product_id) productName FROM `order_details` od where od.id = @Id; ";
                order.Details = _dbConnection.Query<OrderDetailResponse>(detailsQuery, new {Id = order.Id}).ToList();
            }

            return Orders;
        }
    }
}