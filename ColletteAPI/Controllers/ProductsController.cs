/*
 * File: ProductsController.cs
 * Description: Manages HTTP requests for product-related operations, including creating, retrieving, updating, and deleting products for vendors.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Models;
using ColletteAPI.Repositories;
using System.Text.Json;
using ColletteAPI.Services;

namespace ColletteAPI.Controllers
{

    // Controller responsible for managing product-related operations.
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;
        private readonly CloudinaryService _cloudinaryService;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger, CloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _logger = logger;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productDto, [FromQuery] string vendorId)
        {
            _logger.LogInformation($"Received product data: {JsonSerializer.Serialize(productDto)}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid ModelState: {JsonSerializer.Serialize(ModelState)}");
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                UniqueProductId = productDto.UniqueProductId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                VendorId = vendorId,
                IsActive = productDto.IsActive,
                Category = productDto.Category
            };

            if (productDto.Image != null)
            {
                using var stream = productDto.Image.OpenReadStream();
                product.ImageUrl = await _cloudinaryService.UploadImageAsync(stream, productDto.Image.FileName);
            }

            if (await _productRepository.IsUniqueProductIdUnique(product.UniqueProductId))
            {
                await _productRepository.CreateAsync(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest("The provided Unique Product ID is already in use.");
            }
        }

        // Retrieves all products for a specific vendor.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string vendorId)
        {
            var products = await _productRepository.GetAllForVendorAsync(vendorId);
            return Ok(products);
        }

        // Retrieves a specific product by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id, [FromQuery] string vendorId)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null || product.VendorId != vendorId)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromForm] ProductCreateDto productDto, [FromQuery] string vendorId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || existingProduct.VendorId != vendorId)
            {
                return NotFound();
            }

            // Update existing product properties
            existingProduct.UniqueProductId = productDto.UniqueProductId;
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.StockQuantity = productDto.StockQuantity;
            existingProduct.IsActive = productDto.IsActive;
            existingProduct.Category = productDto.Category;

            // Handle image update
            if (productDto.Image != null)
            {
                using var stream = productDto.Image.OpenReadStream();
                var imageUrl = await _cloudinaryService.UploadImageAsync(stream, productDto.Image.FileName);
                existingProduct.ImageUrl = imageUrl;
            }

            await _productRepository.UpdateAsync(id, existingProduct);
            return NoContent();
        }

        // Deletes a product.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id, [FromQuery] string vendorId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null || product.VendorId != vendorId)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}