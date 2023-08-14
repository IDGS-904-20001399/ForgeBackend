
using ErrorOr;
using Forge.Contracts.Supplies;
using Forge.Models;

namespace Forge.Services.Supplies
{
    public interface ISupplyService
    {
        ErrorOr<Created> CreateSupply(Supply supply);
        ErrorOr<Deleted> DeleteSupply(int id);
        ErrorOr<Supply> GetSupply(int id);
        ErrorOr<List<Supply>> GetSupplies();
        ErrorOr<UpsertedSuply> UpsertSupply(Supply supply);
        ErrorOr<Created> BuySupply(BuySupplyRequest request);
        ErrorOr<DetailSupplyResponse> GetDetails(int id);
    }
}