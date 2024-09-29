﻿using ColletteAPI.Data;
using ColletteAPI.Models;
using ColletteAPI.Models.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}