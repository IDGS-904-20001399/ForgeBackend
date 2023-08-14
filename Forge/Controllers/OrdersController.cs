using System.Diagnostics;
using ErrorOr;
using Forge.Contracts.Orders;
using Forge.Contracts.Suppliers;
using Forge.Contracts.Users;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services;
using Forge.Services.Orders;
using Forge.Services.Products;
using Forge.Services.Suppliers;
using Forge.Services.Supplies;
using Forge.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet()]
        [Authorize(Policy = "Admin")]
        public IActionResult GetOrders()
        {
            var ordersResult = _ordersService.Getorders();

            return ordersResult.Match(
                Response => Ok(Response),
                errors => Problem(errors)
            );
        }

        [HttpPost("markasdelivered")]
        [Authorize(Policy = "Admin")]
        public IActionResult MarkAsDelivered(UpdateOrderRequest request)
        {
            var ordersResult = _ordersService.MarkAsDelivered(request);

            return ordersResult.Match(
                Response => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpPost("markasreceived")]
        [Authorize(Policy = "Admin")]
        public IActionResult MarkAsReceived(UpdateOrderRequest request)
        {
            var ordersResult = _ordersService.MarkAsReceived(request);

            return ordersResult.Match(
                Response => NoContent(),
                errors => Problem(errors)
            );
        }

    }

}
