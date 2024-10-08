/*
 * File: UserRegisterDto.cs
 * Description: DTO for user registration requests containing personal and account information.
 */

namespace ColletteAPI.Models.Dtos
{
    /*
     * Class: UserRegisterDto
     * This class is used to encapsulate the data required for user registration requests.
     */
    public class UserRegisterDto
    {
        public string NIC { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string ContactNumber { get; set; }   
        public string Address { get; set; }
    }
}
