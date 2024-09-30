using ColletteAPI.Helpers;
using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;

namespace ColletteAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, AuthService authService, JwtService jwtService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _jwtService = jwtService;
        }
        public async Task<List<User>> GetUsersByType(string userType)
        {
            return await _userRepository.GetUsersByType(userType); // Fetch users by type
        }
        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<AuthResponse> Authenticate(UserLoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsername(loginDto.Username);

            if (user == null || !_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;  // Invalid credentials
            }

            if (string.IsNullOrEmpty(user.UserType))
            {
                throw new Exception("UserType is not set for this user.");  // Ensures UserType is valid
            }

            // Prevent login if the account is not active
            if (!user.IsActive)
            {
                throw new Exception("Your account is not active. Please contact CSR or Administrator.");
            }

            var token = _jwtService.GenerateToken(user.Id, user.UserType);

            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.UserType,
                Email = user.Email,
                Address = user.Address,
                NIC = user.NIC
            };
        }


        public async Task<User> Register(UserRegisterDto registerDto)
        {
            var user = new User
            {
                NIC = registerDto.NIC,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Username = registerDto.Username,
                PasswordHash = _authService.HashPassword(registerDto.Password),
                UserType = registerDto.UserType,
                Address = registerDto.Address,
                ContactNumber = registerDto.ContactNumber,
                IsActive = registerDto.UserType == UserRoles.Customer ? false : true // // Set IsActive to false if the user is a Customer
            };

            await _userRepository.AddUser(user);
            return user;
        }
        public async Task UpdateUser(string id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;
            user.Username = updateDto.Username ?? user.Username;
            user.Address = updateDto.Address ?? user.Address;
            user.IsActive = updateDto.IsActive ?? user.IsActive;

            await _userRepository.UpdateUser(id, user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _userRepository.DeleteUser(id);
        }
        public async Task<List<User>> GetPendingCustomers()
        {
            return await _userRepository.GetPendingCustomers();
        }

    }
}
