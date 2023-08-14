using ErrorOr;
using Forge.Contracts.Orders;

namespace Forge.Services.Orders
{
    public interface IOrdersService
    {
        ErrorOr<List<OrdersResponse>> Getorders();
        ErrorOr<Updated> MarkAsDelivered(UpdateOrderRequest request);
        ErrorOr<Updated> MarkAsReceived(UpdateOrderRequest request);
    }
}