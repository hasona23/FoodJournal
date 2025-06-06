using FoodJournal.API;
using FoodJournal.API.Data;
using FoodJournal.API.Services;
using FoodJournal.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'UserContextConnection' not found."); ;

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

builder.Services.AddAuthentication().AddJwtBearer
    (
        options => options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuer = true,
            //ValidateAudience = true,
            //ValidateLifetime = true,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            //ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"] ?? throw new("JWT issuer not found"),
            ValidAudience = jwtConfig["Audience"] ?? throw new("JWT Audience not found"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"] ?? throw new("JWT encryption KEY not found")))

        }
    );
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

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
    AppUser? user = Activator.CreateInstance<AppUser>();
    user.UserName = "admin";
    user.Email = "admin@gmail.com";
    var result = await signInManager.UserManager.CreateAsync(user, "Admin_123");
    foreach (var item in result.Errors)
    {
        scope.ServiceProvider.GetRequiredService<ILogger<Program>>().LogCritical(item.Description);
    }
    user = await signInManager.UserManager.FindByEmailAsync(user.Email);

    DataSeeder.SeedData(context, user.Id);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(
        options =>
        {
            options.WithTitle("Meal Planner API")
                    .WithTheme(ScalarTheme.BluePlanet)
                    .WithModels(true)
                    ;

        });
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
var userEndPoints = app.MapGroup("/User");
userEndPoints.MapIdentityApi<AppUser>();
userEndPoints.MapPost("/logout", async (SignInManager<AppUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
});

app.UseCors("AllowAll");
app.Run();
