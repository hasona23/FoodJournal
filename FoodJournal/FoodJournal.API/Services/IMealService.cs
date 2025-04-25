using FoodJournal.API.Models;
using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.API.Services
{
    public interface IMealService
    {
        public Task<ResultWithValue<List<MealGetDTOWithFoods>>> GetAllMealsWithFoodAsync();
        public Task<ResultWithValue<MealGetDTOWithFoods>> GetMealByIdAsync(int id);

        public Task<Result> CreateMealAsync(MealCreateDTO mealCreateDTO);
        public Task<Result> UpdateMealAsync(int id, MealUpdateDTO mealUpdateDTO);

        public Task<Result> DeleteMealAsync(int id);
    }
}
