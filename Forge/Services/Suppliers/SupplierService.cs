using System.Data;
using System.Diagnostics;
using Dapper;
using ErrorOr;
using Forge.Models;
using MySql.Data.MySqlClient;
using Forge.ServiceErrors;

namespace Forge.Services.Suppliers
{
    public class SupplierService : ISupplierService
    {

        private readonly MySqlConnection _dbConnection;

        public SupplierService(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public ErrorOr<Created> CreateSupplier(Supplier supplier)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(supplier);

               supplier.Id = _dbConnection.QueryFirstOrDefault<int>("add_supplier", parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Result.Created;
        }

        public ErrorOr<Deleted> DeleteSupplier(int id)
        {
            try
            {
                string query = "UPDATE supplier SET status = 0 WHERE id = @Id";
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

        public ErrorOr<Supplier> GetSupplier(int id)
        {
            try
            {
                string query = "SELECT * FROM supplier WHERE id = @Id AND status = 1";
                Supplier supplier = _dbConnection.QueryFirstOrDefault<Supplier>(query, new { Id = id });
                if (supplier != null){
                    return supplier;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<List<Supplier>> GetSuppliers()
        {
            try
            {
                string query = "SELECT * FROM supplier WHERE status = 1";
                return _dbConnection.Query<Supplier>(query).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<UpsertedRecord> UpsertSupplier(Supplier supplier)
        {
            bool isNewlyCreated = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(supplier);
                parameters.Add("Created", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("upsert_supplier", parameters, commandType: CommandType.StoredProcedure);
                isNewlyCreated = parameters.Get<int>("Created") == 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return new UpsertedRecord(isNewlyCreated);
        }
    }
}