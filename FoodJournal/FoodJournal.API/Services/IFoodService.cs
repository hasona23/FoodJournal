using FoodJournal.API.Models;
using FoodJournal.Shared.Models.Dtos;
namespace FoodJournal.API.Services
{
    public interface IFoodService
    {
        public Task<ResultWithValue<List<FoodGetDTOWithMeals>>> GetAllFoodWithMealsAsync();
        public Task<ResultWithValue<FoodGetDTOWithMeals>> GetFoodByIdAsync(int id);

        public Task<Result> CreateFoodAsync(FoodCreateDTO foodCreateDTO);
        public Task<Result> UpdateFoodAsync(int id, FoodUpdateDTO foodUpdateDTO);
        public Task<Result> DeleteFoodAsync(int id);
    }
}
