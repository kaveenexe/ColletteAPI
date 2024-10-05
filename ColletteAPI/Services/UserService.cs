using ColletteAPI.Helpers;
using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;

namespace ColletteAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, INotificationRepository notificationRepository, AuthService authService, JwtService jwtService)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
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
                throw new UnauthorizedAccessException("Your account is not active. Please contact CSR or Administrator.");
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
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto), "Registration details cannot be null.");
            }

            // Check if the notification repository is correctly initialized
            if (_notificationRepository == null)
            {
                throw new NullReferenceException("Notification repository is not initialized.");
            }

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
                IsActive = registerDto.UserType == UserRoles.Customer ? false : true
            };

            // Make sure AddUser is properly working
            await _userRepository.AddUser(user);

            // Create a notification for CSR
            var notification = new Notification
            {
                Message = "New customer registration needs activation.",
                IsVisibleToCSR = true,
                IsVisibleToAdmin = false,
                IsVisibleToVendor = false,
                IsVisibleToCustomer = false,
            };

            await _notificationRepository.AddNotification(notification); // Ensure this is working without error

            return user;
        }
        public async Task UpdateUser(string id, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Update user information
            user.FirstName = updateDto.FirstName ?? user.FirstName;
            user.LastName = updateDto.LastName ?? user.LastName;
            user.Username = updateDto.Username ?? user.Username;
            user.Address = updateDto.Address ?? user.Address;
            user.IsActive = updateDto.IsActive ?? user.IsActive;

            // Update user in the repository
            await _userRepository.UpdateUser(id, user);

            // Resolve the notification if the customer is activated
            if (user.UserType == UserRoles.Customer && user.IsActive)
            {
                var notification = await _notificationRepository.GetNotificationByMessage("New customer registration needs activation.");
                if (notification != null)
                {
                    await _notificationRepository.MarkNotificationAsSeen(notification.NotificationId); // Mark it as seen
                }
            }
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
