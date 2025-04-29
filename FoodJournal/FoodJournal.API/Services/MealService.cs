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

    public async Task<Result> CreateMealAsync(MealCreateDTO mealCreateDTO)
    {
        Meal meal = new Meal { Name = mealCreateDTO.Name };
        if (mealCreateDTO.Foods != null)
        {
            meal.Foods = [];
            foreach (var foodDTO in mealCreateDTO.Foods)
            {
                Food food = await _context.Foods.FindAsync(foodDTO.Id);
                if (food != null)
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

    public async Task<Result> DeleteMealAsync(int id)
    {
        Meal mealToDelete = await _context.Meals.FindAsync(id);
        if (mealToDelete == null)
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

    public async Task<ResultWithValue<List<MealGetDTOWithFoods>>> GetAllMealsWithFoodAsync()
    {
        try
        {
            var meals = _context.Meals.Include(m => m.Foods);
            var mealsDto = await meals.Select(m =>
            new MealGetDTOWithFoods(m.Id, m.Name, m.MealType, m.GetFoodsDTO())
            ).ToListAsync();

            return ResultWithValue<List<MealGetDTOWithFoods>>.Success(mealsDto);
        }
        catch (Exception ex)
        {
            return ResultWithValue<List<MealGetDTOWithFoods>>.Error(Result.Fail($"Error retrieving meal - {ex.Message}"));
        }

    }

    public async Task<ResultWithValue<MealGetDTOWithFoods>> GetMealByIdAsync(int id)
    {
        try
        {
            Meal meal = await _context.Meals.Include(m => m.Foods).FirstOrDefaultAsync(m => m.Id == id);
            if (meal == null)
            {
                return ResultWithValue<MealGetDTOWithFoods>.Error(Result.NotFoundError());
            }
            return ResultWithValue<MealGetDTOWithFoods>.Success(new MealGetDTOWithFoods(meal.Id, meal.Name, meal.MealType, meal.GetFoodsDTO()));
        }
        catch (Exception ex)
        {
            return ResultWithValue<MealGetDTOWithFoods>.Error(Result.Fail($"Error getting food of ID:#{id} - {ex.Message}"));
        }
    }

    public async Task<Result> UpdateMealAsync(int id, MealUpdateDTO mealUpdateDTO)
    {
        Meal meal = await _context.Meals.FindAsync(id);
        if (meal == null)
        {
            return Result.NotFoundError();
        }
        await _context.Entry(meal).Collection(m => m.Foods).LoadAsync();
        meal.Name = mealUpdateDTO.Name;

        if (mealUpdateDTO.Foods != null)
        {
            meal.Foods.Clear();
            foreach (var foodDTO in mealUpdateDTO.Foods)
            {
                Food food = await _context.Foods.FindAsync(foodDTO.Id);
                if (food != null)
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
            return Result.Fail($"Error Updating Food of ID:#{id} - {ex.Message}");
        }
        return Result.Success();

    }
}
