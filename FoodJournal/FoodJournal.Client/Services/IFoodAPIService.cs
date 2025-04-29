using FoodJournal.Shared.Models.Dtos;

namespace FoodJournal.Client.Services;

public interface IFoodAPIService
{
    public Task<List<FoodGetDTOWithMeals>> FetchAllFoods();
    public Task<FoodGetDTOWithMeals?> FetchFoodById(int id);
    public Task CreateFood(FoodCreateDTO food);
    public Task DeleteFood(int id);
    public Task UpdateFood(int id, FoodUpdateDTO newFood);
}
