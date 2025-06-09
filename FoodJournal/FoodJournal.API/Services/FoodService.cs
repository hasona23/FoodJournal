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

    public async Task<Result> CreateFoodAsync(FoodCreateDTO foodCreateDTO, string userId)
    {
        Food foodToAdd = new Food
        {
            Name = foodCreateDTO.Name,
            UserId = userId,
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

    public async Task<Result> DeleteFoodAsync(int id, string userId)
    {
        Food foodToDelete = await _context.Foods.FindAsync(id);
        if (foodToDelete == null || foodToDelete.UserId != userId)
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

    public async Task<ResultWithValue<List<FoodGetDTO>>> GetAllFoodAsync(string userId)
    {
        return ResultWithValue<List<FoodGetDTO>>.Success(await _context.Foods.Where(f => f.UserId == userId).Select(f => new FoodGetDTO(f.Id, f.Name)).ToListAsync());
    }

    public async Task<ResultWithValue<List<FoodGetDTOWithMeals>>> GetAllFoodWithMealsAsync(string userId)
    {
        try
        {
            var foods = _context.Foods.Include(f => f.Meals).Where(f => f.UserId == userId);
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

    public async Task<ResultWithValue<FoodGetDTOWithMeals>> GetFoodByIdAsync(int id, string userId)
    {
        try
        {
            Food food = await _context.Foods.Include(f => f.Meals).FirstOrDefaultAsync(f => f.Id == id);
            if (food == null || food.UserId != userId)
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

    public async Task<Result> UpdateFoodAsync(FoodUpdateDTO foodUpdateDTO, string userId)
    {
        Food food = await _context.Foods.Include(f => f.Meals).FirstOrDefaultAsync(f => f.Id == foodUpdateDTO.Id);
        if (food == null || food.UserId != userId)
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
            return Result.Fail($"Error Updating Food of ID:#{foodUpdateDTO.Id} - {ex.Message}");
        }
        return Result.Success();

    }
}
