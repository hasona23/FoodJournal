using FoodJournal.API.Data;
using FoodJournal.Shared.Enums;
using FoodJournal.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.API;

public static class DataSeeder
{
    private static Random _rand = new Random();

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
        for (int i = 0; i < 20; i++)
        {
            context.Add(new Food()
            {
                UserId = userId,
                Name = $"Food{i}",
            });
        }

        context.Foods.AddRange(potatoes, eggs, cheese, Olive);
        context.SaveChanges();
        Meal meal = new Meal
        {
            UserId = userId,
            Name = "EggsWithPortato",
            IsStarred = true,
            DateEaten = GetRandomTime(),
            Foods = [eggs, potatoes]
        };
        Meal meal2 = new Meal
        {
            UserId = userId,
            Name = "Cheese-{FANCY-WORD}",
            Foods = [cheese, Olive],
            DateEaten = GetRandomTime()

        };
        Meal meal3 = new Meal
        {
            UserId = userId,
            Name = "Cheesy-Potato",
            Foods = [cheese, potatoes, Olive],
            DateEaten = GetRandomTime()
        };
        context.Meals.AddRange(meal, meal2, meal3);
        var foods = context.Foods.AsQueryable();
        for (int i = 0; i < 20; i++)
        {
            context.Add(new Meal
            {
                UserId = userId,
                Name = $"Meal{i}",
                IsStarred = i % 20 == 0,
                DateEaten = GetRandomTime(),
                Foods = foods.AsQueryable().OrderBy(x => _rand.Next()).Take(_rand.Next(3, 10)).ToList(),
                MealType = Enum.GetValues<MealType>()[_rand.Next(0, Enum.GetValues<MealType>().Length)],
            });


        }
        context.SaveChanges();
        var meals = context.Meals.Include(m => m.Foods).ToList();
        for (int i = 0; i < 50; i++)
        {

            var mealFetched = meals.ElementAt(_rand.Next(0, meals.Count));
            if (mealFetched.Name != $"Meal{i % 10}")
                context.Add(new Meal()
                {
                    UserId = userId,
                    Name = mealFetched.Name,
                    DateEaten = GetRandomTime(),
                    Foods = mealFetched.Foods,
                    MealType = mealFetched.MealType,
                });
        }
        context.SaveChanges();
    }
    private static DateTime GetRandomTime()
    {
        return new DateTime(_rand.Next(2018, 2026), _rand.Next(1, 13), _rand.Next(1, 28));
    }
}
