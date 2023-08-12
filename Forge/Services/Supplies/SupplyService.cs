using System.Data;
using System.Diagnostics;
using Dapper;
using ErrorOr;
using Forge.Models;
using MySql.Data.MySqlClient;
using Forge.ServiceErrors;
using Forge.Contracts.Supplies;

namespace Forge.Services.Supplies
{
    public class SupplyService : ISupplyService
    {

        private readonly MySqlConnection _dbConnection;

        public SupplyService(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public ErrorOr<Created> BuySupply(BuySupplyRequest request)
        {
            // Delete existing rows
            string supplyQuery = "SELECT * FROM supply WHERE id = @Id";

            var supply = _dbConnection.QueryFirstOrDefault<dynamic>(supplyQuery, new {Id = request.SupplyId});

            if (supply != null){
                 var parameters = new
                {
                    BuyDate = DateTime.Now,
                    Quantity = request.Quantity,
                    AvailableUseQuantity = request.Quantity * supply.equivalence,
                    UnitCost = supply.cost,
                    SupplyId = request.SupplyId 
                };

                // Call the stored procedure using Dapper
                _dbConnection.Execute("BuySupply", parameters, commandType: CommandType.StoredProcedure);
            }


            return Result.Created;
        }

        public ErrorOr<Created> CreateSupply(Supply supply)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(supply);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("add_supply", parameters, commandType: CommandType.StoredProcedure);
                supply.Id = parameters.Get<int>("Id");
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Result.Created;
        }

        public ErrorOr<Deleted> DeleteSupply(int id)
        {
            try
            {
                string query = "UPDATE supply SET status = 0 WHERE id = @Id";
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

        public ErrorOr<List<Supply>> GetSupplies()
        {
            try
            {
                string query = "SELECT * FROM supply WHERE status = 1";
                return _dbConnection.Query<Supply>(query).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<Supply> GetSupply(int id)
        {
            try
            {
                string query = "SELECT * FROM supply WHERE id = @Id AND status = 1";
                Supply supply = _dbConnection.QueryFirstOrDefault<Supply>(query, new { Id = id });
                if (supply != null){
                    return supply;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<UpsertedSuply> UpsertSupply(Supply supply)
        {
            bool isNewlyCreated = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(supply);
                parameters.Add("Created", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("upsert_supply", parameters, commandType: CommandType.StoredProcedure);
                isNewlyCreated = parameters.Get<int>("Created") == 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return new UpsertedSuply(isNewlyCreated);
        }
    }
}