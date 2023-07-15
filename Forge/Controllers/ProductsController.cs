using Forge.Contracts.Products;
using Forge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateProduct(CreateProductRequest request)
        {
            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.Category,
                request.Price
            );
            // TODO: Save to database

            var response = new CreateProductRequest(
                product.Name,
                product.Description,
                product.Category,
                product.Price
            );
            return CreatedAtAction(
                actionName: nameof(GetProduct),
                routeValues: new {id = product.Id},
                value: response);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetProduct(Guid id)
        {
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpsertProduct(Guid id, UpsertProductRequest request)
        {
            return Ok(request);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProduct(Guid id)
        {
            return Ok(id);
        }
    }
}
