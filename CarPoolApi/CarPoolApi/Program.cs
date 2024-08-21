using Application.Interfaces;
using Application.Services.Implementations;
using Core.Interfaces;
using Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Configure the URLs to be used by the application
builder.WebHost.UseUrls("https://localhost:7026", "http://localhost:5026");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// JWT Authentication configuration
var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniPool", Version = "v1" });

    // Configure JWT authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add Cosmos DB configuration
builder.Services.AddSingleton(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("COSMOSDB_CONNECTION_STRING");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Cosmos DB connection string is not configured properly.");
    }
    return new CosmosClient(connectionString);
});

// Register repositories with database and container names from configuration
builder.Services.AddScoped<IUserRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("USER_CONTAINER_NAME");
    return new CosmosDbUserRepository(cosmosClient, databaseName, containerName);
});

builder.Services.AddScoped<IBookingRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("BOOKING_CONTAINER_NAME");
    return new CosmosDbBookingRepository(cosmosClient, databaseName, containerName);
});

builder.Services.AddScoped<IRideRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("RIDE_CONTAINER_NAME");
    return new CosmosDbRideRepository(cosmosClient, databaseName, containerName);
});

builder.Services.AddScoped<IPaymentRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("PAYMENT_CONTAINER_NAME");
    return new CosmosDbPaymentRepository(cosmosClient, databaseName, containerName);
});

builder.Services.AddScoped<IRatingRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("RATING_CONTAINER_NAME");
    return new CosmosDbRatingRepository(cosmosClient, databaseName, containerName);
});

builder.Services.AddScoped<IScheduleRepository>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
    var containerName = Environment.GetEnvironmentVariable("SCHEDULE_CONTAINER_NAME");
    return new CosmosDbScheduleRepository(cosmosClient, databaseName, containerName);
});

// Register application services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRideService, RideService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IRatingService, RatingService>();
// builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder.Services.AddScoped<IPasswordHasher<Entities.DTOs.User>, PasswordHasher<Entities.DTOs.User>>();

var app = builder.Build();

// Ensure Swagger UI is only available in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
