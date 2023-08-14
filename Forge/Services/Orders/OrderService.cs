using System.Data;
using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;
using Forge.Contracts.Products;
using Forge.Services.Orders;
using Forge.Contracts.Orders;
using Forge.Contracts.Customers;

public class OrderService : IOrdersService
{
    private readonly MySqlConnection _dbConnection;

    public OrderService(MySqlConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public ErrorOr<List<Forge.Contracts.Orders.OrdersResponse>> Getorders()
    {
            string orderQuery = "SELECT o.*, (SELECT SUM(quantity * price) FROM order_details WHERE order_id = o.id) subtotal, (SELECT subtotal + delivery_fee) total FROM `order` o";
            var Orders = _dbConnection.Query<Forge.Contracts.Orders.OrdersResponse>(orderQuery).ToList();
            foreach(var order in Orders){
                string detailsQuery = "SELECT od.*, (SELECT name from product where id = od.product_id) productName FROM `order_details` od where od.id = @Id; ";
                order.Details = _dbConnection.Query<Forge.Contracts.Customers.OrderDetailResponse>(detailsQuery, new {Id = order.Id}).ToList();
                string customerQuery = "SELECT c.*, (SELECT email from user where id = c.user_id) email FROM customer c WHERE user_id = @Id";
                order.Customer = _dbConnection.QueryFirstOrDefault<CustomerResponse>(customerQuery, new {Id = order.UserId});
            }

            return Orders;
    }

    public ErrorOr<Updated> MarkAsDelivered(UpdateOrderRequest request)
    {
        string updateQuery = "UPDATE `order` SET status = @Estatus WHERE id = @Id";
        _dbConnection.Execute(updateQuery, new {Estatus = "Enviada", Id = request.OrderId});
        return Result.Updated;
    }

    public ErrorOr<Updated> MarkAsReceived(UpdateOrderRequest request)
    {
        string updateQuery = "UPDATE `order` SET status = @Estatus WHERE id = @Id";
        _dbConnection.Execute(updateQuery, new {Estatus = "Entregada", Id = request.OrderId});
        return Result.Updated;
    }
}