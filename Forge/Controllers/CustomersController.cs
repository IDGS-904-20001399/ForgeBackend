using ErrorOr;
using Forge.Contracts.Customers;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Customers;
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

        [HttpPost("signup")]
        public IActionResult CreateCustomer(CreateCustomerRequest request)
        {
            ErrorOr<Customer> requestToCustomerResult = Customer.From(request);

            if (requestToCustomerResult.IsError)
            {
                return Problem(requestToCustomerResult.Errors);
            }

            if (request.Password != request.ConfirmPassword)
            {

                return Problem(new List<Error>(){Errors.User.PasswordsNotEqual});
            }

            var customer = requestToCustomerResult.Value;
            ErrorOr<ErrorOr.Created> createCustomerResult = _customerService.CreateCustomer(customer);

            return createCustomerResult.Match(
                created => Ok(request),
                errors => Problem(errors)
            );
        }


        [HttpPost("orders")]
        [Authorize(Policy = "Customer")]
        public IActionResult GetOrder(OrderRequest request)
        {
            var ordersResult = _customerService.GetOrders(request);

            return ordersResult.Match(
                Response => Ok(Response),
                errors => Problem(errors)
            );

        }

        [HttpPost("updatedata")]
        [Authorize(Policy = "Customer")]
        public IActionResult UpdateData(UpdateDataRequest request)
        {
            var ordersResult = _customerService.UpdateCustomerdata(request);

            return ordersResult.Match(
                Response => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "Customer")]
        public IActionResult GetCustomer(int id)
        {
            var customerResult = _customerService.GetCustomer(id);

            return customerResult.Match(
                Customer => Ok(MapCustomerResponse(Customer)),
                errors => Problem(errors)
            );

        }

        private static CustomerResponse MapCustomerResponse(Customer customer)
        {
            return new CustomerResponse(
                customer.Id,
                customer.Email,
                customer.Names,
                customer.Lastnames,
                customer.Address,
                customer.Phone
            );
        }

        // private IActionResult CreatedAtGetCustomer(Customer customer)
        // {
        //     return CreatedAtAction(
        //         actionName: nameof(GetCustomer),
        //         routeValues: new { id = product.Id },
        //         value: MapProductResponse(product));
        // }


    }
}
