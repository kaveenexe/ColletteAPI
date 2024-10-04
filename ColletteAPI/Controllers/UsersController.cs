using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Inject IUserService instead of IUserRepository

        public UsersController(IUserService userService) // Inject IUserService
        {
            _userService = userService;
        }

        // Get all vendors
        [HttpGet("vendors")]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _userService.GetUsersByType("Vendor");
            return Ok(vendors);
        }

        // Get all CSRs
        [HttpGet("csrs")]
        public async Task<IActionResult> GetCSRs()
        {
            var csrs = await _userService.GetUsersByType("CSR");
            return Ok(csrs);
        }

        // Get all customers
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _userService.GetUsersByType("Customer");
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserById(id); // Use _userService here
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // Update user (Vendor, CSR, Customer)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Invalid payload.");
            }

            try
            {
                await _userService.UpdateUser(id, updateDto);
                return Ok("User updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        // Delete user (Vendor, CSR, Customer)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok("User deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // Get all pending customers
        [HttpGet("pending-customers")]
        public async Task<IActionResult> GetPendingCustomers()
        {
            var pendingCustomers = await _userService.GetPendingCustomers();
            return Ok(pendingCustomers);
        }

    }
}
