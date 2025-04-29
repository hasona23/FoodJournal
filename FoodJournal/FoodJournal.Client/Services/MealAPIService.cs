using FoodJournal.Shared.Models.Dtos;
using System.Net.Http.Json;

namespace FoodJournal.Client.Services;

public class MealAPIService : IMealAPIService
{
    private IHttpClientFactory _httpClientFactory;

    public MealAPIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }

    public async Task<List<MealGetDTOWithFoods>> FetchAllMeals()
    {

        return await GetClient().GetFromJsonAsync<List<MealGetDTOWithFoods>>("Meals/") ?? [];
    }
    public async Task<MealGetDTOWithFoods?> FetchMealById(int id)
    {
        return await GetClient().GetFromJsonAsync<MealGetDTOWithFoods>($"Meals/{id}");
    }

    public async Task DeleteMeal(int id)
    {
        await GetClient().DeleteAsync($"Meals/{id}");
    }
    public async Task UpdateMeal(int id, MealUpdateDTO meal)
    {
        await GetClient().PutAsJsonAsync($"Meals/{id}", meal);
    }

    public async Task CreateMeal(MealCreateDTO meal)
    {
        await GetClient().PostAsJsonAsync<MealCreateDTO>("Meals/", meal);
    }
    public HttpClient GetClient()
    {

        return _httpClientFactory.CreateClient("API");

    }

}
