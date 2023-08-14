using System.Diagnostics;
using ErrorOr;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Products;
using Forge.Services.Supplies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class SuppliesController : ApiController
    {
        private readonly ISupplyService _supplyService;

        public SuppliesController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }

        [HttpPost]
        [Authorize(Policy = "Stocker")]
        public IActionResult CreateSupply(CreateSupplyRequest request)
        {
            ErrorOr<Supply> requestToSupplyResult = Supply.From(request);

            if (requestToSupplyResult.IsError)
            {
                return Problem(requestToSupplyResult.Errors);
            }

            var supply = requestToSupplyResult.Value;
            ErrorOr<ErrorOr.Created> createSupplyResult = _supplyService.CreateSupply(supply);

            return createSupplyResult.Match(
                created => CreatedAtGetSupply(supply),
                errors => Problem(errors)
            );

        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = "Stocker")]
        public IActionResult GetSupply(int id)
        {
            ErrorOr<Supply> getSupplyResult = _supplyService.GetSupply(id);
            return getSupplyResult.Match(
                Supply => Ok(MapSupplyResponse(Supply)),
                errors => Problem(errors)
            );
        }

        [HttpGet()]
        [Authorize(Policy = "Stocker")]
        public IActionResult GetSupplies()
        {
            ErrorOr<List<Supply>> getSuppliesResult = _supplyService.GetSupplies();
            return getSuppliesResult.Match(
                Supplies => Ok(MapSuppliesResponses(Supplies)),
                errors => Problem(errors)
            );
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "Stocker")]
 
        public IActionResult UpsertSupply(int id, CreateSupplyRequest request)
        {
            ErrorOr<Supply> requestToSupplyResult = Supply.From(id, request);

            if (requestToSupplyResult.IsError)
            {
                return Problem(requestToSupplyResult.Errors);
            }

            var supply = requestToSupplyResult.Value;

            ErrorOr<UpsertedSuply> upserteSupplyResult = _supplyService.UpsertSupply(supply);

            return upserteSupplyResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedAtGetSupply(supply) : NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Stocker")]
        public IActionResult DeleteSupply(int id)
        {
            ErrorOr<Deleted> deleteSupplyResult = _supplyService.DeleteSupply(id);
            return deleteSupplyResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpPost("buy")]
        [Authorize(Policy = "Stocker")]
        public IActionResult SetDetails(BuySupplyRequest request)
        {
            ErrorOr<ErrorOr.Created> BuyResult = _supplyService.BuySupply(request);
            return BuyResult.Match(
                created => NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpGet("details/{id:int}")]
        [Authorize(Policy = "Stocker")]
        public IActionResult GetDetails(int id)
        {
            ErrorOr<Contracts.Supplies.DetailSupplyResponse> DetailsResult = _supplyService.GetDetails(id);
            return DetailsResult.Match(
                Details => Ok(Details),
                errors => Problem(errors)
            );
        }

        private static List<SupplyResponse> MapSuppliesResponses(List<Supply> supplies)
        {
            List<SupplyResponse> responses = new List<SupplyResponse>();
            foreach (var supply in supplies)
            {
                responses.Add(MapSupplyResponse(supply));
            }
            return responses;
        }

        private static SupplyResponse MapSupplyResponse(Supply supply)
        {
            return new SupplyResponse(
                supply.Id,
                supply.Name,
                supply.Cost,
                supply.BuyUnit,
                supply.UseUnit,
                supply.Equivalence,
                supply.Image,
                supply.Stock,
                supply.InventoryStatus
            );
        }
        private IActionResult CreatedAtGetSupply(Supply supply)
        {
            return CreatedAtAction(
                actionName: nameof(GetSupply),
                routeValues: new { id = supply.Id },
                value: MapSupplyResponse(supply));
        }
    }

}
