using ErrorOr;
using Forge.Contracts.Products;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Controllers
{
    public class ProductsController : ApiController
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
            ErrorOr<ErrorOr.Created> createProductResult = _productService.CreateProduct(product);

            return createProductResult.Match(
                created => CreatedAtGetProduct(product),
                errors => Problem(errors)
            );

        }


        [HttpGet("{id:guid}")]
        public IActionResult GetProduct(Guid id)
        {
            ErrorOr<Product> getProductResult = _productService.GetProduct(id);
            return getProductResult.Match(
                Product => Ok(MapProductResponse(Product)),
                errors => Problem(errors)
            );
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

            ErrorOr<UpsertedProduct> upserteProductResult = _productService.UpsertProduct(product);

            // TODO: Return 201 if new product was created
            return upserteProductResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedAtGetProduct(product) : NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProduct(Guid id)
        {
            ErrorOr<Deleted> deleteProductResult = _productService.DeleteProduct(id);
            return deleteProductResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }

        private static ProductResponse MapProductResponse(Product product)
        {
            return new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price
            );
        }
        private IActionResult CreatedAtGetProduct(Product product)
        {
            return CreatedAtAction(
                actionName: nameof(GetProduct),
                routeValues: new { id = product.Id },
                value: MapProductResponse(product));
        }
    }

}
