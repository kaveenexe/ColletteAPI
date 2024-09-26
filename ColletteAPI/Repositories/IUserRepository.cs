using System.Threading.Tasks;
using ColletteAPI.Models.Domain;
namespace ColletteAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(string id);
        Task AddUser(User user);
        Task UpdateUser(string id, User user);
        Task DeleteUser(string id);
    }
}
