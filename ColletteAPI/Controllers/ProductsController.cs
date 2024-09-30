using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Models;
using ColletteAPI.Repositories;
using System.Text.Json;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product, [FromQuery] string vendorId)
        {
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
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest("The provided Unique Product ID is already in use.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string vendorId)
        {
            var products = await _productRepository.GetAllForVendorAsync(vendorId);
            return Ok(products);
        }

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
        public async Task<IActionResult> UpdateProduct(string id, Product product, [FromQuery] string vendorId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null || existingProduct.VendorId != vendorId)
            {
                return NotFound();
            }
            product.VendorId = vendorId;
            await _productRepository.UpdateAsync(id, product);
            return NoContent();
        }

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