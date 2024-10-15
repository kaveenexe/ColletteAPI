/*
 * File: UserUpdateDto.cs
 * Description: DTO for updating existing user information.
 */

namespace ColletteAPI.Models.Dtos
{
    /*
     * Class: UserUpdateDto
     * This class is used to encapsulate the data required for updating a user's profile or account information.
     */
    public class UserUpdateDto
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}
