/*
 * File: JwtService.cs
 * Description: This class handles the generation of JWT tokens used for authenticating and authorizing users in the system.
 */
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ColletteAPI.Helpers
{
    /*
     * Class: JwtService
     * This service generates JWT tokens with user-specific claims such as user ID and role. 
     * It also reads configuration settings such as the secret key, issuer, and audience.
     */
    public class JwtService
    {
        private readonly string _secretKey;     // Secret key used to sign the JWT token
        private readonly string _issuer;        // The issuer of the token (your application)
        private readonly string _audience;      // The audience for which the token is intended
        private readonly int _expiryMinutes;    // The expiration time of the token in minutes

        /*
         * Constructor: JwtService
         * Initializes the JwtService class by reading the JWT settings from the configuration file.
         * 
         * Parameters:
         *  - configuration: The application's configuration, used to get JWT-related settings (SecretKey, Issuer, Audience, ExpiryMinutes).
         */
        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["JWT:SecretKey"];
            _issuer = configuration["JWT:Issuer"];
            _audience = configuration["JWT:Audience"];
            _expiryMinutes = int.Parse(configuration["JWT:ExpiryMinutes"]);
        }

        /*
         * Method: GenerateToken
         * Generates a JWT token for a user, embedding their user ID and role into the token claims.
         * 
         * Parameters:
         *  - userId: The ID of the user for whom the token is being generated.
         *  - role: The role of the user (e.g., Administrator, Vendor, CSR).
         * 
         * Returns:
         *  - A signed JWT token string containing the user's claims and expiration time.
         * 
         * Throws:
         *  - ArgumentNullException: Thrown if either the userId or role parameters are null or empty.
         */
        public string GenerateToken(string userId, string role)
        {
            // Ensure that userId and role are not null or empty
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),  // Ensure role is not null
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Add a unique identifier for the token (JTI)
            };

            // Create a symmetric security key using the secret key from the configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            // Specify the HMAC SHA-256 algorithm for signing the token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiryMinutes),   // Set the token's expiration time
                signingCredentials: creds   // Sign the token using the credentials created
            );
            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
