/*
 * File: UserRepository.cs
 * Description: This file implements the IUserRepository interface and contains methods for interacting with the MongoDB database for user-related operations.
 */

using ColletteAPI.Data;
using System.Threading.Tasks;
using ColletteAPI.Models.Domain;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace ColletteAPI.Repositories
{
    /*
     * Class: UserRepository
     * This class interacts with the MongoDB database to perform user-related operations such as adding, updating, and deleting users.
     */
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        /*
         * Constructor: UserRepository
         * Initializes the MongoDB collection for users based on the database name from the configuration.
         * 
         * Parameters:
         *  - client: MongoDB client for database connection.
         *  - configuration: Application configuration to access database connection details.
         */
        public UserRepository(IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(configuration.GetSection("MongoDB:DatabaseName").Value);
            _users = database.GetCollection<User>("Users");
        }

        /*
         * Method: GetUserByUsername
         * Retrieves a user from the database based on the username.
         * 
         * Parameters:
         *  - username: The username of the user to retrieve.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation. The task result contains the user object.
         */
        public async Task<User> GetUserByUsername(string username)
        {
            return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
        }

        /*
         * Method: GetUserById
         * Retrieves a user from the database by their ID.
         * 
         * Parameters:
         *  - id: The ID of the user to retrieve.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation. The task result contains the user object.
         */
        public async Task<User> GetUserById(string id)
        {
            // Use ObjectId conversion for MongoDB when fetching by Id
            var objectId = ObjectId.Parse(id);
            return await _users.Find(user => user.Id == objectId.ToString()).FirstOrDefaultAsync();
        }

        /*
         * Method: GetUsersByType
         * Retrieves all users from the database based on the user type (e.g., Admin, Vendor, Customer).
         * 
         * Parameters:
         *  - userType: The type of users to retrieve.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation. The task result contains a list of users.
         */
        public async Task<List<User>> GetUsersByType(string userType)
        {
            return await _users.Find(user => user.UserType == userType).ToListAsync();
        }

        /*
         * Method: AddUser
         * Inserts a new user into the MongoDB collection.
         * 
         * Parameters:
         *  - user: The user object to add.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation.
         */
        public async Task AddUser(User user)
        {
            await _users.InsertOneAsync(user);
        }

        /*
         * Method: UpdateUser
         * Replaces an existing user document with an updated user object based on the user's ID.
         * 
         * Parameters:
         *  - id: The ID of the user to update.
         *  - user: The updated user object.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation.
         */
        public async Task UpdateUser(string id, User user)
        {
            var objectId = ObjectId.Parse(id);
            await _users.ReplaceOneAsync(u => u.Id == objectId.ToString(), user);
        }

        /*
         * Method: DeleteUser
         * Deletes a user from the database based on their ID.
         * 
         * Parameters:
         *  - id: The ID of the user to delete.
         * 
         * Returns:
         *  - A Task representing the asynchronous operation.
         */
        public async Task DeleteUser(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _users.DeleteOneAsync(u => u.Id == objectId.ToString());
        }

        /*
         * Method: GetPendingCustomers
         * Retrieves a list of customers with inactive accounts (i.e., customers whose IsActive status is false).
         * 
         * Returns:
         *  - A Task representing the asynchronous operation. The task result contains a list of pending customers.
         */
        public async Task<List<User>> GetPendingCustomers()
        {
            return await _users.Find(user => user.UserType == "Customer" && user.IsActive == false).ToListAsync();
        }
    }
}