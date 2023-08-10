using System.Diagnostics;
using ErrorOr;
using Forge.Contracts.Suppliers;
using Forge.Contracts.Users;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services;
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
    public class SuppliersController : ApiController
    {
        private readonly ISupplierService _suppliersService;

        public SuppliersController(ISupplierService supplierService)
        {
            _suppliersService = supplierService;;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult CreateSupplier(CreateSupplierRequest request)
        {
            ErrorOr<Supplier> requestToSupplierResult = Supplier.From(request);

            if (requestToSupplierResult.IsError)
            {
                return Problem(requestToSupplierResult.Errors);
            }

            var supplier = requestToSupplierResult.Value;
            ErrorOr<ErrorOr.Created> createUserResult = _suppliersService.CreateSupplier(supplier);

            return createUserResult.Match(
                created => CreatedAtGetSupplier(supplier),
                errors => Problem(errors)
            );

        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult GetSupplier(int id)
        {
            ErrorOr<Supplier> getSupplierResult = _suppliersService.GetSupplier(id);
            return getSupplierResult.Match(
                Supplier => Ok(MapSupplierResponse(Supplier)),
                errors => Problem(errors)
            );
        }

        [HttpGet()]
        [Authorize(Policy = "Admin")]
        public IActionResult GetSuppliers()
        {
            ErrorOr<List<Supplier>> getSuppliersResult = _suppliersService.GetSuppliers();
            return getSuppliersResult.Match(
                Suppliers => Ok(MapSuppliersResponses(Suppliers)),
                errors => Problem(errors)
            );
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpsertSupplier(int id, CreateSupplierRequest request)
        {
            ErrorOr<Supplier> requestToSupplierResult =  Supplier.From(id, request);

            if (requestToSupplierResult.IsError){
                return Problem(requestToSupplierResult.Errors);
            }

            var supplier = requestToSupplierResult.Value;

            ErrorOr<UpsertedRecord> upsertedSupplierResult = _suppliersService.UpsertSupplier(supplier);

            return upsertedSupplierResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedAtGetSupplier(supplier) : NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteSupplier(int id)
        {
            ErrorOr<Deleted> deleteSupplierResult = _suppliersService.DeleteSupplier(id);
            return deleteSupplierResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }

        private static List<SupplierResponse> MapSuppliersResponses(List<Supplier> suppliers){
            List<SupplierResponse> responses = new ();
            foreach(var supplier in suppliers){
                responses.Add(MapSupplierResponse(supplier));
            }
            return responses;
        }

        private static SupplierResponse MapSupplierResponse(Supplier supplier)
        {
            return new SupplierResponse(
                supplier.Id,
                supplier.Name,
                supplier.Email,
                supplier.Phone
            );
        }
        private IActionResult CreatedAtGetSupplier(Supplier supplier)
        {
            return CreatedAtAction(
                actionName: nameof(GetSupplier),
                routeValues: new { id = supplier.Id },
                value: MapSupplierResponse(supplier));
        }
    }

}
