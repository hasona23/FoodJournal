using FoodJournal.Shared.Models.Dtos;
using System.Net.Http.Json;

namespace FoodJournal.Client.Services;

public class FoodAPIService : IFoodAPIService
{
    private IHttpClientFactory _httpFactory;
    public FoodAPIService(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }
    public async Task CreateFood(FoodCreateDTO food)
    {
        await GetClient().PostAsJsonAsync("Food/", food);
    }

    public async Task DeleteFood(int id)
    {
        await GetClient().DeleteAsync($"Food/{id}");
    }

    public async Task<List<FoodGetDTOWithMeals>> FetchAllFoods()
    {
        return await GetClient().GetFromJsonAsync<List<FoodGetDTOWithMeals>>("Food/") ?? [];
    }

    public async Task<FoodGetDTOWithMeals?> FetchFoodById(int id)
    {
        return await GetClient().GetFromJsonAsync<FoodGetDTOWithMeals>($"Food/{id}");
    }

    public async Task UpdateFood(int id, FoodUpdateDTO newFood)
    {
        await GetClient().PutAsJsonAsync<FoodUpdateDTO>($"Food/{id}", newFood);
    }
    public HttpClient GetClient()
    {
        return _httpFactory.CreateClient("API");
    }
}
