using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ColletteAPI.Models;
using ColletteAPI.Repositories;

namespace ColletteAPI.Controllers
{
    [ApiController]
    [Route("api/customer/products")]
    public class CustomerProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CustomerProductsController> _logger;

        public CustomerProductsController(IProductRepository productRepository, ILogger<CustomerProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products for customers");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}