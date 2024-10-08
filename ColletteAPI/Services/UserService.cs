/*
 * File: UserService.cs
 * Description: This file contains the implementation of the UserService class, 
 * which provides various user-related operations, such as authentication, 
 * user registration, and notification handling for the ColletteAPI project.
*/

using ColletteAPI.Helpers;
using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;
using ColletteAPI.Repositories;

namespace ColletteAPI.Services
{
    /*
     * Class: UserService
     * This service provides methods to handle user authentication, 
     * registration, and account management operations.
     */
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        // Constructor to initialize dependencies via Dependency Injection (DI).
        public UserService(IUserRepository userRepository, INotificationRepository notificationRepository, AuthService authService, JwtService jwtService)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _authService = authService;
            _jwtService = jwtService;
        }

        /*
         * Method: GetUsersByType
         * Fetches all users based on the provided user type.
         */
        public async Task<List<User>> GetUsersByType(string userType)
        {
            return await _userRepository.GetUsersByType(userType); // Fetch users by type
        }

        /*
         * Method: GetUserById
         * Retrieves a user by their unique ID.
         */
        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetUserById(id);
        }

        /*
         * Method: Authenticate
         * Authenticates a user based on their username and password, and returns an authentication token.
         */
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
                throw new UnauthorizedAccessException("Your account is not active. Please contact CSR or Administrator."); // Account is not active and send a message to the user
            }

            // Generate JWT token for the authenticated user
            var token = _jwtService.GenerateToken(user.Id, user.UserType);

            // Return the authentication response with the user details and token
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

        /*
         * Method: Register
         * Registers a new user and triggers a notification for CSR if the user is a customer.
         */
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

            // Create a new User object based on the registration details
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

            // Save the new user to the database
            await _userRepository.AddUser(user);

            // Create a notification for CSR to activate the customer account
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
        /*
         * Method: UpdateUser
         * Updates the details of an existing user.
         */
        public async Task UpdateUser(string id, UserUpdateDto updateDto)
        {
            // Fetch the user to be updated from the repository
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

            // Save the updated user details to the database
            await _userRepository.UpdateUser(id, user);

            // Mark the notification as resolved if the customer account is activated
            if (user.UserType == UserRoles.Customer && user.IsActive)
            {
                var notification = await _notificationRepository.GetNotificationByMessage("New customer registration needs activation.");
                if (notification != null)
                {
                    await _notificationRepository.MarkNotificationAsSeen(notification.NotificationId); // Mark it as seen
                }
            }
        }

        /*
         * Method: DeleteUser
         * Deletes a user by their unique ID.
         */
        public async Task DeleteUser(string id)
        {
            // Fetch the user to be deleted
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            // Delete the user from the database
            await _userRepository.DeleteUser(id);
        }

        /*
         * Method: GetPendingCustomers
         * Fetches the list of customers whose accounts are pending activation.
         */
        public async Task<List<User>> GetPendingCustomers()
        {
            return await _userRepository.GetPendingCustomers();
        }

    }
}
