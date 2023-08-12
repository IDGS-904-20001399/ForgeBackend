using System.Data;
using System.Diagnostics;
using Dapper;
using ErrorOr;
using Forge.Models;
using Forge.Services;
using MySql.Data.MySqlClient;
using Forge.ServiceErrors;

namespace Forge.Services.Users
{
    public class UserService : IUserService
    {

        private readonly MySqlConnection _dbConnection;

        public UserService(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public ErrorOr<Created> CreateUser(User user)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(user);
                parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("add_user", parameters, commandType: CommandType.StoredProcedure);
                user.Id = parameters.Get<int>("Id");
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Result.Created;
        }

        public ErrorOr<Deleted> DeleteUser(int id)
        {
            try
            {
                string query = "UPDATE user SET status = 0 WHERE id = @Id";
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

        public ErrorOr<User> GetUser(int id)
        {
            try
            {
                string query = "SELECT u.*, (SELECT role_id from roles_users where user_id = u.id) role_id, (SELECT name FROM role r where r.id = role_id ) role FROM user u WHERE u.id = 1 AND u.status = 1";
                User user = _dbConnection.QueryFirstOrDefault<User>(query, new { Id = id });
                if (user != null){
                    return user;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<List<User>> GetUsers()
        {
            try
            {
                string query = "SELECT u.*, (SELECT role_id from roles_users where user_id = u.id) role_id, (SELECT name FROM role r where r.id = role_id ) role FROM user u WHERE u.status = 1 AND u.id in (SELECT user_id from roles_users where role_id <> 4)";
                return _dbConnection.Query<User>(query).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Supply.NotFound;
        }

        public ErrorOr<UpsertedRecord> UpsertUser(User user)
        {
            bool isNewlyCreated = false;
            try
            {
                var parameters = new DynamicParameters();
                parameters.AddDynamicParams(user);
                parameters.Add("Created", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _dbConnection.Execute("upsert_user", parameters, commandType: CommandType.StoredProcedure);
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