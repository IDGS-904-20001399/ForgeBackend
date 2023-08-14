using ErrorOr;
using Forge.Contracts.Customers;
using Forge.Models;

namespace Forge.Services.Customers
{
    public interface ICustomerService
    {
        ErrorOr<BuyResponse> Buy(BuyRequest request);
        ErrorOr<Created> CreateCustomer(Customer customer);
        ErrorOr<List<OrdersResponse>> GetOrders(OrderRequest request);
        ErrorOr<Updated> UpdateCustomerdata(UpdateDataRequest request);
    }
}