using System.Data;
using ErrorOr;
using Forge.Models;
using Forge.ServiceErrors;
using MySql.Data.MySqlClient;
using Dapper;
using System.Diagnostics;
using Forge.Contracts.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Forge.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly MySqlConnection _dbConnection;
        private readonly IConfiguration _configuration;

        public LoginService(MySqlConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }
        public ErrorOr<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                bool authenticated = false;
                string message = $"The user {request.Email} does not exists.";
                string token = "";

                string query = "SELECT count(*) FROM user WHERE email = @Email";
                int nUsers = _dbConnection.QueryFirstOrDefault<int>(query, new { request.Email });
                if (nUsers >= 1)
                {
                    string authenticationQuery = "SELECT id FROM user WHERE email = @Email and password = @Password";
                    int id = _dbConnection.QueryFirstOrDefault<int>(authenticationQuery, new { request.Email, request.Password });
                    if (id != 0)
                    {
                        string rolesQuery = "SELECT name from role where id = (select role_id from roles_users where user_id = @Id)";
                        var roles = _dbConnection.Query<string>(rolesQuery, new { Id = id }).ToList();
                        authenticated = true;
                        message = "User authenticated.";
                        token = generateToken(request.Email, roles);
                    }
                    else
                    {
                        message = "The password is wrong please try again.";
                    }
                }
                return new LoginResponse(authenticated, token, message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("--------------------------------ERROR-----------------------------------------");
                Debug.WriteLine(e);
            }
            return Errors.Product.NotFound;
        }

        private string generateToken(string email, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            string secretKey = _configuration.GetValue<string>("JWT:Key");
            string audience = _configuration.GetValue<string>("JWT:Audience") ?? "";
            string issuer = _configuration.GetValue<string>("JWT:Issuer") ?? "";
            var key = Encoding.ASCII.GetBytes(secretKey);
            var rolesClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();
            Claim[] claims = new Claim[rolesClaims.Length + 1];
            claims[0] = new Claim(ClaimTypes.Name, email);
            for (int i = 0; i < rolesClaims.Length; i++)
            {
                claims[i + 1] = rolesClaims[i];
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = audience, // Set the correct audience here
                Issuer = issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }
}