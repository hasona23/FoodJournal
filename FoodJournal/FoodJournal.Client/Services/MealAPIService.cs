using FoodJournal.Shared.Models.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FoodJournal.Client.Services;

public class MealAPIService : IMealAPIService
{
    private IHttpClientFactory _httpClientFactory;
    private NavigationManager _navManager;
    private IUserService _userService;

    public MealAPIService(IHttpClientFactory httpClientFactory, NavigationManager navManager, IUserService userService)
    {
        _httpClientFactory = httpClientFactory;
        _navManager = navManager;
        _userService = userService;
    }

    public async Task<List<MealGetDTOWithFoods>> FetchAllMeals()
    {

        var result = await (await GetClient()).GetAsync("Meals/");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<List<MealGetDTOWithFoods>>();
        }
        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navManager.NavigateTo("/login");
        }
        return [];
    }
    public async Task<MealGetDTOWithFoods?> FetchMealById(int id)
    {
        var result = await (await GetClient()).GetAsync($"Meals/{id}");
        if (result.IsSuccessStatusCode)
            return await result.Content.ReadFromJsonAsync<MealGetDTOWithFoods>();
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
        return null;
    }

    public async Task DeleteMeal(int id)
    {
        var result = await (await GetClient()).DeleteAsync($"Meals/{id}");
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
    }
    public async Task UpdateMeal(int id, MealUpdateDTO meal)
    {
        var result = await (await GetClient()).PutAsJsonAsync($"Meals/{id}", meal);
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
    }

    public async Task CreateMeal(MealCreateDTO meal)
    {
        var result = await (await GetClient()).PostAsJsonAsync("Meals/", meal);
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
    }
    public async Task<HttpClient> GetClient()
    {

        var client = _httpClientFactory.CreateClient("API");
        var token = await _userService.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            _navManager.NavigateTo("/login");
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{token}");
        }
        return client;

    }

}
