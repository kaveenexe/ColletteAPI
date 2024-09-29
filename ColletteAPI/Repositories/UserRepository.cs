using ColletteAPI.Data;
using System.Threading.Tasks;
using ColletteAPI.Models.Domain;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace ColletteAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _users = database.GetCollection<User>("Users");
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            // Use ObjectId conversion for MongoDB when fetching by Id
            var objectId = ObjectId.Parse(id);
            return await _users.Find(user => user.Id == objectId.ToString()).FirstOrDefaultAsync();
        }

        public async Task AddUser(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUser(string id, User user)
        {
            var objectId = ObjectId.Parse(id);
            await _users.ReplaceOneAsync(u => u.Id == objectId.ToString(), user);
        }

        public async Task DeleteUser(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _users.DeleteOneAsync(u => u.Id == objectId.ToString());
        }
    }
}