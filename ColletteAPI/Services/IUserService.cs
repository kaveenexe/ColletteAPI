/*
 * File: IUserService.cs
 * Description: This file defines the IUserService interface, which contains method signatures for user-related operations such as authentication, registration, updating, and fetching users.
 */

using ColletteAPI.Models.Domain;
using ColletteAPI.Models.Dtos;

namespace ColletteAPI.Services
{
    /*
     * Interface: IUserService
     * This interface provides the contract for the user-related services, including methods for
     * user management such as authentication, registration, fetching users, updating, and deletion.
     */
    public interface IUserService
    {
        Task<List<User>> GetUsersByType(string userType);
        Task<User> GetUserById(string id);
        Task<AuthResponse> Authenticate(UserLoginDto loginDto);
        Task<User> Register(UserRegisterDto registerDto);
        Task UpdateUser(string id, UserUpdateDto updateDto); // Add update method signature
        Task DeleteUser(string id);
        Task<List<User>> GetPendingCustomers();
    }
}
