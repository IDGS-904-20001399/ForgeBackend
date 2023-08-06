using ErrorOr;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.Contracts.Users;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Login;
using Forge.Services.Products;
using Forge.Services.Supplies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class LoginController : ApiController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            ErrorOr<LoginResponse> requestToLoginRespondResponse = _loginService.Login(request);

            if (requestToLoginRespondResponse.IsError)
            {
                return Problem(requestToLoginRespondResponse.Errors);
            }

            var loginResponse = requestToLoginRespondResponse.Value;
            return Ok(loginResponse);
        }


    }

}
