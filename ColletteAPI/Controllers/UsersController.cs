/*
 * File: UsersController.cs
 * Description: This controller handles user-related operations such as retrieving users by type, updating, deleting, and managing pending customers.
 */

using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColletteAPI.Controllers
{
    /*
     * Controller: UsersController
     * Handles user-related actions such as fetching users by type (Vendor, CSR, Customer), updating, deleting, and retrieving pending customers.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Inject IUserService instead of IUserRepository

        /*
         * Constructor: UsersController
         * Initializes a new instance of the UsersController class.
         * 
         * Parameters:
         *  - userService: The IUserService to interact with user-related data.
         */
        public UsersController(IUserService userService) // Inject IUserService
        {
            _userService = userService;
        }

        /*
         * Method: GetVendors
         * Retrieves all users with the type "Vendor".
         */
        [HttpGet("vendors")]
        public async Task<IActionResult> GetVendors()
        {
            var vendors = await _userService.GetUsersByType("Vendor");
            return Ok(vendors);
        }

        /*
         * Method: GetCSRs
         * Retrieves all users with the type "CSR".
         */
        [HttpGet("csrs")]
        public async Task<IActionResult> GetCSRs()
        {
            var csrs = await _userService.GetUsersByType("CSR");
            return Ok(csrs);
        }

        /*
         * Method: GetCustomers
         * Retrieves all users with the type "Customer".
         */
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _userService.GetUsersByType("Customer");
            return Ok(customers);
        }

        /*
         * Method: GetUserById
         * Retrieves a user by their unique ID.
         */
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

        /*
         * Method: UpdateUser
         * Updates user details (Vendor, CSR, Customer) by their ID.
         * 
         * Parameters:
         *  - id: The ID of the user to update.
         *  - updateDto: The data transfer object containing the updated user details.
         */
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

        /*
         * Method: DeleteUser
         * Deletes a user by their ID (Vendor, CSR, Customer).
         * 
         * Parameters:
         *  - id: The ID of the user to delete.
         */
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

        /*
         * Method: GetPendingCustomers
         * Retrieves all customers whose accounts are not yet active (pending customers).
         */
        [HttpGet("pending-customers")]
        public async Task<IActionResult> GetPendingCustomers()
        {
            var pendingCustomers = await _userService.GetPendingCustomers();
            return Ok(pendingCustomers);
        }

    }
}
