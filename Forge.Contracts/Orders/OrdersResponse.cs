using Forge.Contracts.Customers;

namespace Forge.Contracts.Orders
{
    public class OrdersResponse{
        public OrdersResponse()
        {
        }
        public int Id {get; set;}
        public string Payment {get; set;}
        public string Status {get; set;}
        public double DeliveryFee {get; set;}
        public DateTime Date {get; set;}
        public double Subtotal {get; set;}
        public double Total {get; set;}
        public List<OrderDetailResponse> Details {get; set;}

        public CustomerResponse Customer{get; set;}
        public int UserId{get; set;}

    }
}