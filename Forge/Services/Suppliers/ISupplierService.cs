using ErrorOr;
using Forge.Models;

namespace Forge.Services.Suppliers
{
    public interface ISupplierService
    {

        ErrorOr<Created> CreateSupplier(Supplier supplier);
        ErrorOr<Deleted> DeleteSupplier(int id);
        ErrorOr<Supplier> GetSupplier(int id);
        ErrorOr<List<Supplier>> GetSuppliers();
        ErrorOr<UpsertedRecord> UpsertSupplier(Supplier supplier);
    }
}