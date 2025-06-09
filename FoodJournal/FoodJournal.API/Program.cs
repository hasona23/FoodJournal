using FoodJournal.API;
using FoodJournal.API.Data;
using FoodJournal.API.Services;
using FoodJournal.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'UserContextConnection' not found.");

// Add services to the container.
var config = builder.Configuration;
var jwtConfig = builder.Configuration.GetSection("Jwt");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("AppDb");
});

builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();

    var user = new AppUser
    {
        UserName = "user@example.com",
        Email = "user@example.com"
    };
    await signInManager.UserManager.CreateAsync(user, "Password_123");

    var adminUser = new AppUser
    {
        UserName = "admin@example.com",
        Email = "admin@example.com"
    };
    await signInManager.UserManager.CreateAsync(adminUser, "Admin_123");

    var foundUser = await signInManager.UserManager.FindByEmailAsync(adminUser.Email);
    DataSeeder.SeedData(context, foundUser.Id);
}

// Configure the HTTP request pipeline - CORRECT ORDER IS IMPORTANT
app.UseCors("AllowAll"); // CORS should be early in the pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Meal Planner API")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithModels(true);
    });
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Identity endpoints
var userEndPoints = app.MapGroup("api/User");
userEndPoints.MapIdentityApi<AppUser>();
userEndPoints.MapPost("/logout", async (SignInManager<AppUser> signInManager, [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
});

app.Run();