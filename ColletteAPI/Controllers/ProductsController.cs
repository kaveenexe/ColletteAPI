using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ColletteAPI.Models;
using ColletteAPI.Repositories;
using System.Text.Json;
using System.Security.Claims;

namespace ColletteAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        private bool ValidateVendorId(string routeVendorId)
        {
            var claimVendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return claimVendorId == routeVendorId;
        }

        [HttpPost("api/vendors/{vendorId}/products")]
        public async Task<ActionResult<Product>> CreateProduct(string vendorId, [FromBody] Product product)
        {
            if (!ValidateVendorId(vendorId))
            {
                return Forbid("You are not authorized to perform this action.");
            }

            _logger.LogInformation($"Received product data: {JsonSerializer.Serialize(product)}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid ModelState: {JsonSerializer.Serialize(ModelState)}");
                return BadRequest(ModelState);
            }

            product.VendorId = vendorId;

            if (await _productRepository.IsUniqueProductIdUnique(product.UniqueProductId))
            {
                await _productRepository.CreateAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { vendorId, id = product.Id }, product);
            }
            else
            {
                return BadRequest("The provided Unique Product ID is already in use.");
            }
        }

        [HttpGet("api/vendors/{vendorId}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string vendorId)
        {
            if (!ValidateVendorId(vendorId))
            {
                return Forbid("You are not authorized to perform this action.");
            }

            var products = await _productRepository.GetAllByVendorIdAsync(vendorId);
            return Ok(products);
        }

        [HttpGet("api/vendors/{vendorId}/products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(string vendorId, string id)
        {
            if (!ValidateVendorId(vendorId))
            {
                return Forbid("You are not authorized to perform this action.");
            }

            var product = await _productRepository.GetByIdAndVendorIdAsync(id, vendorId);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("api/vendors/{vendorId}/products/{id}")]
        public async Task<IActionResult> UpdateProduct(string vendorId, string id, Product product)
        {
            if (!ValidateVendorId(vendorId))
            {
                return Forbid("You are not authorized to perform this action.");
            }

            if (id != product.Id)
            {
                return BadRequest("ID in the route does not match the ID in the product object.");
            }

            var existingProduct = await _productRepository.GetByIdAndVendorIdAsync(id, vendorId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            product.VendorId = vendorId;
            await _productRepository.UpdateAsync(id, product);
            return NoContent();
        }

        [HttpDelete("api/vendors/{vendorId}/products/{id}")]
        public async Task<IActionResult> DeleteProduct(string vendorId, string id)
        {
            if (!ValidateVendorId(vendorId))
            {
                return Forbid("You are not authorized to perform this action.");
            }

            var product = await _productRepository.GetByIdAndVendorIdAsync(id, vendorId);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}