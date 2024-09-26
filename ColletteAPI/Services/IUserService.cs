using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    public interface IUserService
    {
        Task<AuthResponse> Authenticate(UserLoginDto loginDto);
        Task<User> Register(UserRegisterDto registerDto);
    }
}
