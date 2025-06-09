using FoodJournal.API.Data;
using FoodJournal.API.Models;
using FoodJournal.Shared.Models;
using FoodJournal.Shared.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.API.Services;

public class MealService : IMealService
{
    AppDbContext _context;
    public MealService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> CreateMealAsync(MealCreateDTO mealCreateDTO, string userId)
    {
        Meal meal = new Meal { Name = mealCreateDTO.Name, UserId = userId, IsStarred = mealCreateDTO.IsStarred, DateEaten = mealCreateDTO.DateEaten };
        if (mealCreateDTO.Foods != null)
        {
            meal.Foods = [];

            foreach (var foodDTO in mealCreateDTO.Foods)
            {
                Food food = await _context.Foods.FindAsync(foodDTO.Id);
                if (food != null && food.UserId == userId)
                {
                    meal.Foods.Add(food);
                }

            }
        }

        await _context.Meals.AddAsync(meal);

        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error Adding meal {mealCreateDTO.ToString} - {ex.Message}");
        }
        return Result.Success();
    }

    public async Task<Result> DeleteMealAsync(int id, string userId)
    {

        Meal mealToDelete;

        mealToDelete = await _context.Meals.FindAsync(id);

        if (mealToDelete == null || mealToDelete.UserId != userId)
        {
            return Result.NotFoundError();
        }
        _context.Remove(mealToDelete);
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error Deleting meal of ID:#{id} - {ex}");
        }
        return Result.Success();
    }

    public async Task<ResultWithValue<List<MealGetDTOWithFoods>>> GetAllMealsWithFoodAsync(string userId)
    {
        try
        {

            var meals = _context.Meals.Include(m => m.Foods).Where(m => m.UserId == userId);
            var mealsDto = await meals.Select(m =>
            new MealGetDTOWithFoods(m.Id, m.Name, m.IsStarred, m.DateEaten, m.MealType, m.GetFoodsDTO())
            ).ToListAsync();

            return ResultWithValue<List<MealGetDTOWithFoods>>.Success(mealsDto);
        }
        catch (Exception ex)
        {
            return ResultWithValue<List<MealGetDTOWithFoods>>.Error(Result.Fail($"Error retrieving meal - {ex.Message}"));
        }

    }

    public async Task<ResultWithValue<MealGetDTOWithFoods>> GetMealByIdAsync(int id, string userId)
    {
        try
        {

            Meal meal = await _context.Meals.Include(m => m.Foods).FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null || meal.UserId != userId)
            {
                return ResultWithValue<MealGetDTOWithFoods>.Error(Result.NotFoundError());
            }
            return ResultWithValue<MealGetDTOWithFoods>.Success(new MealGetDTOWithFoods(meal.Id, meal.Name, meal.IsStarred, meal.DateEaten, meal.MealType, meal.GetFoodsDTO()));
        }
        catch (Exception ex)
        {
            return ResultWithValue<MealGetDTOWithFoods>.Error(Result.Fail($"Error getting food of ID:#{id} - {ex.Message}"));
        }
    }

    public async Task<Result> UpdateMealAsync(MealUpdateDTO mealUpdateDTO, string userId)
    {
        Meal meal = await _context.Meals.FindAsync(mealUpdateDTO.Id);
        if (meal == null || meal.UserId != userId)
        {
            return Result.NotFoundError();
        }
        await _context.Entry(meal).Collection(m => m.Foods).LoadAsync();
        meal.Name = mealUpdateDTO.Name;
        meal.MealType = mealUpdateDTO.MealType;
        meal.DateEaten = mealUpdateDTO.DateEaten;
        meal.IsStarred = mealUpdateDTO.IsStarred;
        if (mealUpdateDTO.Foods != null)
        {
            meal.Foods.Clear();
            foreach (var foodDTO in mealUpdateDTO.Foods)
            {
                Food food = await _context.Foods.FindAsync(foodDTO.Id);
                if (food != null && food.UserId == userId)
                {
                    meal.Foods.Add(food);
                }
            }
        }
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error Updating Food of ID:#{mealUpdateDTO.Id} - {ex.Message}");
        }
        return Result.Success();

    }
}
