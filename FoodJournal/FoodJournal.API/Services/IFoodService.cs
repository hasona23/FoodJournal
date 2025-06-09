using FoodJournal.API.Models;
using FoodJournal.Shared.Models.Dtos;
namespace FoodJournal.API.Services
{
    public interface IFoodService
    {
        public Task<ResultWithValue<List<FoodGetDTOWithMeals>>> GetAllFoodWithMealsAsync(string userId);
        public Task<ResultWithValue<List<FoodGetDTO>>> GetAllFoodAsync(string userId);
        public Task<ResultWithValue<FoodGetDTOWithMeals>> GetFoodByIdAsync(int id, string userId);

        public Task<Result> CreateFoodAsync(FoodCreateDTO foodCreateDTO, string userId);
        public Task<Result> UpdateFoodAsync(FoodUpdateDTO foodUpdateDTO, string userId);
        public Task<Result> DeleteFoodAsync(int id, string userId);
    }
}
