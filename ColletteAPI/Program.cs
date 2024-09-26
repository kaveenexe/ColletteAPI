using ColletteAPI.Data;
using ColletteAPI.Helpers;
using ColletteAPI.Repositories;
using ColletteAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserService, UserService>(); //Register User Service
builder.Services.AddScoped<IUserRepository, UserRepository>(); //Register User Repository
builder.Services.AddSingleton<AuthService>(); //Register Auth Service
builder.Services.AddSingleton<JwtService>(); //Register JWT Service

// MongoDB connection ===
// Register MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register the MongoDB context
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
