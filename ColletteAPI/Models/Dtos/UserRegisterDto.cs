namespace ColletteAPI.Models.Dtos
{
    public class UserRegisterDto
    {
        public string NIC { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Address { get; set; }
    }
}
