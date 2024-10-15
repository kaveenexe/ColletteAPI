/*
 * File: MongoDbContext.cs
 * Description: Defines the MongoDB context for interacting with the database and collections in the ColletteAPI project.
 */
using ColletteAPI.Data;
using ColletteAPI.Models;
using ColletteAPI.Models.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

/*
 * Class: MongoDbContext
 * This class acts as the context for interacting with the MongoDB database.
 * It provides access to various MongoDB collections (Users, Products, Orders, etc.) used in the application.
 */
public class MongoDbContext
{
    private readonly IMongoDatabase _database;  //// The database object for interacting with MongoDB.

    /*
     * Constructor: MongoDbContext
     * Initializes a new instance of the MongoDbContext class.
     * It takes IOptions<MongoDbSettings> to configure the connection to the MongoDB instance.
     */
    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        // Create a new MongoDB client using the provided connection string.
        var client = new MongoClient(settings.Value.ConnectionString);
        // Get the specified MongoDB database.
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    // * Properties to access different MongoDB collections. *
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");

    public IMongoCollection<Inventory> Inventories => _database.GetCollection<Inventory>("Inventories");

    public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");

}