using FoodJournal.API.Data;
using FoodJournal.Shared.Models;

namespace FoodJournal.API;

public static class DataSeeder
{
    public static void SeedData(AppDbContext context, string userId)
    {
        if (context.Foods.Any())
            return;

        Food eggs = new Food
        {
            UserId = userId,
            Name = "Eggs",
        };
        Food potatoes = new Food
        {
            UserId = userId,
            Name = "Potatoes",
        };
        Food cheese = new Food
        {
            UserId = userId,
            Name = "Cottage-Cheese"
        };
        Food Olive = new Food
        {
            UserId = userId,
            Name = "Olives"
        };


        context.Foods.AddRange(potatoes, eggs, cheese, Olive);

        Meal meal = new Meal
        {
            UserId = userId,
            Name = "EggsWithPortato",
            Foods = [eggs, potatoes]
        };
        Meal meal2 = new Meal
        {
            UserId = userId,
            Name = "Cheese-{FANCY-WORD}",
            Foods = [cheese, Olive]
        };
        Meal meal3 = new Meal
        {
            UserId = userId,
            Name = "Cheesy-Potato",
            Foods = [cheese, potatoes, Olive]
        };
        context.Meals.AddRange(meal, meal2, meal3);
        context.SaveChanges();
    }
}
