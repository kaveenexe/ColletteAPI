namespace ColletteAPI.Models.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}
