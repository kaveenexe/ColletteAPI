/*
 * File: AuthController.cs
 * Description: This controller handles authentication-related operations such as user login and registration.
 */

using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColletteAPI.Controllers
{
    /*
     * Controller: AuthController
     * Handles authentication actions such as user login and registration.
     */
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; // Dependency on IUserService

        /*
         * Constructor: AuthController
         * Initializes a new instance of the AuthController class.
         */
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /*
         * Method: Login
         * Authenticates a user and returns a JWT token if successful.
         * 
         * Parameters:
         *  - loginDto: The login details (username and password) provided by the user.
         * 
         * Returns:
         *  - A JWT token and user details if authentication is successful, or Unauthorized if credentials are invalid.
         */
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var authResponse = await _userService.Authenticate(loginDto);
                if (authResponse == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                return Ok(authResponse); // Returns JWT token and user details
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message); // Returns 403 if access is forbidden
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        /*
         * Method: Register
         * Registers a new user in the system.
         * 
         * Parameters:
         *  - registerDto: The registration details provided by the user (including username, password, and other details).
         * 
         * Returns:
         *  - The newly registered user's ID, username, and user type.
         */
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var user = await _userService.Register(registerDto);
            return Ok(new { user.Id, user.Username, user.UserType }); // Returns essential user details after registration
        }
    }
}
