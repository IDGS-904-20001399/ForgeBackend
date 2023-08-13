using ErrorOr;
using Forge.Contracts.Customers;

namespace Forge.Services.Customer
{
    public interface ICustomerService
    {
        ErrorOr<BuyResponse> Buy(BuyRequest request);
    }
}