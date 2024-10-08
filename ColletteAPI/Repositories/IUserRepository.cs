/*
 * File: IUserRepository.cs
 * Description: This interface defines the contract for the UserRepository, which includes methods for interacting with user data in MongoDB.
 */

using System.Threading.Tasks;
using ColletteAPI.Models.Domain;
namespace ColletteAPI.Repositories
{
    /*
     * Interface: IUserRepository
     * Defines the methods for interacting with user-related data in the database.
     */
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(string id);
        Task<List<User>> GetUsersByType(string userType);
        Task AddUser(User user);
        Task UpdateUser(string id, User user);
        Task DeleteUser(string id);
        Task<List<User>> GetPendingCustomers();
    }
}
