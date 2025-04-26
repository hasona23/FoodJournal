using FoodJournal.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.API.Data
{
    public sealed class AppDbContext : IdentityDbContext<AppUser>
    {
        //Stores food to be chosen from. Not Instances


        public DbSet<Meal> Meals { get; set; }


        public DbSet<Food> Foods { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Meal>()
                .HasMany(meal => meal.Foods)
                .WithMany(food => food.Meals)
                .UsingEntity(e => e.ToTable("MealFoodJoinTable"));


        }
    }
}
