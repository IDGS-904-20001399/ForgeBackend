using System.Data;
using System.Diagnostics;
using Dapper;
using ErrorOr;
using Forge.Models;
using Forge.Services;
using MySql.Data.MySqlClient;
using Forge.ServiceErrors;
using Forge.Contracts.Users;
using Org.BouncyCastle.X509.Store;

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
                if (IsEmailTaken(0, user.Email))
                {
                    return Errors.User.EmailTaken;
                }

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

        public bool IsEmailTaken(int userId, string email)
        {
            string query = "SELECT COUNT(*) FROM user WHERE email = @Email AND id <> @Id";
            int count = _dbConnection.QuerySingle<int>(query, new { Email = email, Id = userId });

            return count > 0;
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
                if (user != null)
                {
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

                if (IsEmailTaken(user.Id, user.Email))
                {
                    return Errors.User.EmailTaken;
                }
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

        public ErrorOr<Updated> UpdateEmail(UpdateEmailRequest request)
        {

            if (IsEmailTaken(request.UserId, request.Email))
            {
                return Errors.User.EmailTaken;
            }
            string query = "UPDATE `user` set email = @Email where id = @id";
            _dbConnection.Execute(query, new { Email = request.Email, Id = request.UserId });

            return Result.Updated;
        }

        public ErrorOr<UpdatePasswordResponse> UpdatePassword(UpdatePasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return Errors.User.PasswordsNotEqual;
            }
            bool Success = true;
            string message = "Password updated successfully";
            string query = "SELECT COUNT(*) FROM `user` WHERE id = @UserId AND password = @Password";

            int result = _dbConnection.ExecuteScalar<int>(query, new { UserId = request.UserId, Password = request.CurrentPassword });
            if (result == 1)
            {
                string updateQuery = "UPDATE `user` set password = @Password where id = @id";
                _dbConnection.Execute(updateQuery, new { Password = request.NewPassword, Id = request.UserId });
            }else{
                Success = false;
                message = "The current password is wrong";
            }

            return new UpdatePasswordResponse(Success, message);
        }
    }
}