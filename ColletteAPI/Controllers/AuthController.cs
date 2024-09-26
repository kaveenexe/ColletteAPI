﻿using ColletteAPI.Models.Dtos;
using ColletteAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColletteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var authResponse = await _userService.Authenticate(loginDto);
            if (authResponse == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(authResponse); // Returns JWT token and user details
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var user = await _userService.Register(registerDto);
            return Ok(new { user.Id, user.Username, user.UserType });
        }
    }
}
