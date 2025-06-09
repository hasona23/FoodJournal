using FoodJournal.API.Models;
using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.API.Services
{
    public interface IMealService
    {
        public Task<ResultWithValue<List<MealGetDTOWithFoods>>> GetAllMealsWithFoodAsync(string userId);
        public Task<ResultWithValue<MealGetDTOWithFoods>> GetMealByIdAsync(int id, string userId);

        public Task<Result> CreateMealAsync(MealCreateDTO mealCreateDTO, string userId);
        public Task<Result> UpdateMealAsync(MealUpdateDTO mealUpdateDTO, string userId);

        public Task<Result> DeleteMealAsync(int id, string userId);
    }
}
