/*
 * File: UserLoginDto.cs
 * Description: DTO for user login requests containing the username and password.
 */

namespace ColletteAPI.Models.Dtos
{
    /*
     * Class: UserLoginDto
     * This class is used to encapsulate the data required for user login requests.
     */
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
