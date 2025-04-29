using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.Client.Services;

public interface IMealAPIService
{
    public Task<List<MealGetDTOWithFoods>> FetchAllMeals();
    public Task<MealGetDTOWithFoods?> FetchMealById(int id);
    public Task CreateMeal(MealCreateDTO meal);
    public Task DeleteMeal(int id);
    public Task UpdateMeal(int id, MealUpdateDTO newMeal);
}
