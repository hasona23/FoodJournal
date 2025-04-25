using FoodJournal.API.Data;
using FoodJournal.API.Models;
using FoodJournal.Shared.Models;
using FoodJournal.Shared.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FoodJournal.API.Services;

public class FoodService : IFoodService
{
    AppDbContext _context;
    public FoodService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result> CreateFoodAsync(FoodCreateDTO foodCreateDTO)
    {
        Food foodToAdd = new Food
        {
            Name = foodCreateDTO.Name,
            Meals = [],
        };
        await _context.Foods.AddAsync(foodToAdd);
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error Adding food {foodCreateDTO.ToString} - {ex.Message}");
        }
        return Result.Success();
    }

    public async Task<Result> DeleteFoodAsync(int id)
    {
        Food foodToDelete = await _context.Foods.FindAsync(id);
        if (foodToDelete == null)
        {
            return Result.NotFoundError();
        }
        _context.Remove(foodToDelete);
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error Deleting food of ID:#{id} - {ex}");
        }
        return Result.Success();
    }

    public async Task<ResultWithValue<List<FoodGetDTOWithMeals>>> GetAllFoodWithMealsAsync()
    {
        try
        {
            var foods = _context.Foods.Include(f => f.Meals);
            var foodsDto = await foods.Select(f =>
            new FoodGetDTOWithMeals(f.Id, f.Name, f.GetMealsDTO())
            ).ToListAsync();

            return ResultWithValue<List<FoodGetDTOWithMeals>>.Success(foodsDto);
        }
        catch (Exception ex)
        {
            return ResultWithValue<List<FoodGetDTOWithMeals>>.Error(Result.Fail($"Error retrieving food - {ex.Message}"));
        }

    }

    public async Task<ResultWithValue<FoodGetDTOWithMeals>> GetFoodByIdAsync(int id)
    {
        try
        {
            Food food = await _context.Foods.Include(f => f.Meals).FirstOrDefaultAsync(f => f.Id == id);
            if (food == null)
            {
                return ResultWithValue<FoodGetDTOWithMeals>.Error(Result.NotFoundError());
            }
            return ResultWithValue<FoodGetDTOWithMeals>.Success(new FoodGetDTOWithMeals(food.Id, food.Name, food.GetMealsDTO()));
        }
        catch (Exception ex)
        {
            return ResultWithValue<FoodGetDTOWithMeals>.Error(Result.Fail($"Error getting food of ID:#{id} - {ex.Message}"));
        }
    }

    public async Task<Result> UpdateFoodAsync(int id, FoodUpdateDTO foodUpdateDTO)
    {
        Food food = await _context.Foods.Include(f => f.Meals).FirstOrDefaultAsync(f => f.Id == id);
        if (food == null)
        {
            return Result.NotFoundError();
        }
        food.Name = foodUpdateDTO.Name;

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
