using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersByType(string userType);
        Task<User> GetUserById(string id);
        Task<AuthResponse> Authenticate(UserLoginDto loginDto);
        Task<User> Register(UserRegisterDto registerDto);
        Task UpdateUser(string id, UserUpdateDto updateDto); // Add update method signature
        Task DeleteUser(string id);
    }
}
