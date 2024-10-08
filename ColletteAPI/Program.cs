/*
 * File: Program.cs
 * Description: This is the main entry point for configuring and running the ColletteAPI application.
 */
using MongoDB.Driver;
using ColletteAPI.Data;
using ColletteAPI.Helpers;
using ColletteAPI.Repositories;
using ColletteAPI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetSection("MongoDB:ConnectionString").Value));   // Register MongoDB client as a singleton.
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration.GetSection("MongoDB:DatabaseName").Value)); // Register MongoDB database instance.

// Register repository and service classes for dependency injection.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>(); //Register User Service
builder.Services.AddScoped<IUserRepository, UserRepository>(); //Register User Repository
builder.Services.AddScoped<INotificationRepository, NotificationRepository>(); // Register Notification Repository
builder.Services.AddSingleton<AuthService>(); //Register Auth Service
builder.Services.AddSingleton<JwtService>(); //Register JWT Service

// Register IOrderService
builder.Services.AddScoped<IOrderService, OrderService>();

// Register IOrderRepository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register ProductRepository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register ICartService
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Register ICategoryService
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Register ICategoryRepository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Register IInventoryService
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Register IInventoryRepository
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Register ICommentService
builder.Services.AddScoped<ICommentService, CommentService>();

// Register ICommentRepository
builder.Services.AddScoped<ICommentRepository, CommentRepository>();


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", // Define a CORS policy
        builder => builder.WithOrigins("http://localhost:3000") // Allow the React app running on localhost:3000.
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Use CORS to allow requests from the allowed origins.
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); // Run the application.