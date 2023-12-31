using System.Diagnostics;
using ErrorOr;
using Forge.Contracts.Orders;
using Forge.Contracts.Suppliers;
using Forge.Contracts.Users;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services;
using Forge.Services.Dashboard;
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
    public class DashboardController : ApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("customersummary")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetCustomerSummmary()
        {
            return Ok(_dashboardService.GetCustomerSummaries());
        }

        [HttpGet("customersummarytotal")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetCustomerSummmaryTotal()
        {
            return Ok(_dashboardService.GetCustomerSummariesTotal());
        }

        [HttpGet("monthlyproductsolds")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetMonthlyProductSold()
        {
            return Ok(_dashboardService.GetMonthlyProductSolds());
        }

        [HttpGet("productssold")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetProductSold()
        {
            return Ok(_dashboardService.GetProductsSold());
        }

        [HttpGet("productssoldtotal")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetProductSoldTotal()
        {
            return Ok(_dashboardService.GetProductsSoldTotal());
        }

        [HttpGet("productinventory")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetProductInventory()
        {
            return Ok(_dashboardService.GetProductInventory());
        }

        [HttpGet("productsummary")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetProductSummaries()
        {
            return Ok(_dashboardService.GetProductSummaries());
        }

        [HttpGet("statistics")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetStatistics()
        {
            return Ok(_dashboardService.GetStatistics());
        }

        [HttpGet("statisticstotal")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetStatisticsTotal()
        {
            return Ok(_dashboardService.GetStatisticsSummary());
        }

        [HttpGet("suplyinventory")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetSupplyInventory()
        {
            return Ok(_dashboardService.GetSupplyInventory());
        }

        [HttpGet("supplysummary")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetSupplySummaries()
        {
            return Ok(_dashboardService.GetSupplySummary());
        }

        [HttpGet("supplysummarytotal")]
        [Authorize(Policy = "Orders")]
        public IActionResult GetSupplySummariesTotal()
        {
            return Ok(_dashboardService.GetSupplySummaryTotal());
        }

    }

}
