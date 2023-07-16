using Forge.Contracts.Products;
using Forge.Models;
using Forge.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error(){
            return Problem();
        }
    }
}
