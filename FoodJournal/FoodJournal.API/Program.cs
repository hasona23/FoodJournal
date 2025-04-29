using FoodJournal.API;
using FoodJournal.API.Data;
using FoodJournal.API.Services;
using FoodJournal.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'UserContextConnection' not found."); ;

// Add services to the container.
var config = builder.Configuration;
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

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>(options =>
{
    //TODO: CHANGE CONGIFURATION
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
}
).AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options => options.ExpireTimeSpan = TimeSpan.FromHours(1));

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
    var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
    AppUser? user = new AppUser() { UserName = "admin", Email = "admin@gmail.com" };
    var result = await signInManager.UserManager.CreateAsync(user, "admin123");
    user = await signInManager.UserManager.FindByEmailAsync(user.Email);
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
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

app.UseAuthorization();

app.MapControllers();

app.MapGroup("/account").MapIdentityApi<AppUser>();
app.UseCors("AllowAll");
app.Run();
