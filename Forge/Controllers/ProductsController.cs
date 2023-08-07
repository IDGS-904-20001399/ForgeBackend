﻿using ErrorOr;
using Forge.Contracts.Products;
using Forge.Contracts.Supplies;
using Forge.Models;
using Forge.ServiceErrors;
using Forge.Services.Products;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "Admin")]
        public IActionResult CreateProduct(CreateProductRequest request)
        {
            ErrorOr<Product> requestToProductResult = Product.From(request);

            if (requestToProductResult.IsError)
            {
                return Problem(requestToProductResult.Errors);
            }

            var product = requestToProductResult.Value;
            ErrorOr<ErrorOr.Created> createProductResult = _productService.CreateProduct(product);

            return createProductResult.Match(
                created => CreatedAtGetProduct(product),
                errors => Problem(errors)
            );

        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = "Logged")]
        public IActionResult GetProduct(int id)
        {
            ErrorOr<Product> getProductResult = _productService.GetProduct(id);
            return getProductResult.Match(
                Product => Ok(MapProductResponse(Product)),
                errors => Problem(errors)
            );
        }

        [HttpGet()]
        [Authorize(Policy = "Logged")]
        public IActionResult GetProducts()
        {
            ErrorOr<List<Product>> getProductResult = _productService.GetProducts();
            return getProductResult.Match(
                Products => Ok(MapProductResponses(Products)),
                errors => Problem(errors)
            );
        }


        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult UpsertProduct(int id, UpsertProductRequest request)
        {
            ErrorOr<Product> requestToProductResult = Product.From(id, request);

            if (requestToProductResult.IsError)
            {
                return Problem(requestToProductResult.Errors);
            }

            var product = requestToProductResult.Value;

            ErrorOr<UpsertedProduct> upserteProductResult = _productService.UpsertProduct(product);

            return upserteProductResult.Match(
                upserted => upserted.isNewlyCreated ? CreatedAtGetProduct(product) : NoContent(),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            ErrorOr<Deleted> deleteProductResult = _productService.DeleteProduct(id);
            return deleteProductResult.Match(
                deleted => NoContent(),
                errors => Problem(errors)
            );
        }

        private static List<ProductResponse> MapProductResponses(List<Product> products)
        {
            List<ProductResponse> responses = new List<ProductResponse>();
            foreach (var product in products)
            {
                responses.Add(MapProductResponse(product));
            }
            return responses;
        }

        private static ProductResponse MapProductResponse(Product product)
        {
            return new ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                product.Image
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
