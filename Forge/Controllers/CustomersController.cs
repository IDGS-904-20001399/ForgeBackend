using ErrorOr;
using Forge.Contracts.Customers;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Customer;
using Forge.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("buy")]
        [Authorize(Policy = "Customer")]
        public IActionResult Buy(BuyRequest request)
        {
            ErrorOr<BuyResponse> BuyResult = _customerService.Buy(request);
            return BuyResult.Match(
                response => Ok(response),
                errors => Problem(errors)
            );
        }


    }
}
