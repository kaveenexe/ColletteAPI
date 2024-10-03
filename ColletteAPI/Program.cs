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
    new MongoClient(builder.Configuration.GetSection("MongoDB:ConnectionString").Value));
builder.Services.AddSingleton<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration.GetSection("MongoDB:DatabaseName").Value));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>(); //Register User Service
builder.Services.AddScoped<IUserRepository, UserRepository>(); //Register User Repository
builder.Services.AddScoped<INotificationRepository, NotificationRepository>(); // Register Notification Repository
builder.Services.AddSingleton<AuthService>(); //Register Auth Service
builder.Services.AddSingleton<JwtService>(); //Register JWT Service


// Register ProductRepository
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddScoped<ICartRepository, CartRepository>();


// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Use CORS
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

app.Run();