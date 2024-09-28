namespace ColletteAPI.Models.Dtos
{
    public class UserUpdateDto
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
    }
}
