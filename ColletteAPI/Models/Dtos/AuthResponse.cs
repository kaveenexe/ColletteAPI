/*
 * File: AuthResponse.cs
 * Description: Defines the response object returned after a successful authentication (login).
 */

namespace ColletteAPI.Models.Dtos
{
    /*
     * Class: AuthResponse
     * This class represents the response data returned to the client after successful user authentication.
     */
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string NIC { get; set; }
    }
}
