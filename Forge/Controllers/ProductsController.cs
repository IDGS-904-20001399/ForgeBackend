using Forge.Contracts.Products;
using Forge.Models;
using Forge.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

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
            _productService.CreateProduct(product);

            var response = new ProductResponse(
                product.Id,
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
            Product product = _productService.GetProduct(id);
            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price
            );
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpsertProduct(Guid id, UpsertProductRequest request)
        {
            var product = new Product(
                id,
                request.Name,
                request.Description,
                request.Category,
                request.Price
            );

            _productService.UpsertProduct(product);
            // TODO: Return 201 if new product was created
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProduct(Guid id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
