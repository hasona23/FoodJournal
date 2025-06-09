using FoodJournal.Shared.Models.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FoodJournal.Client.Services;

public class FoodAPIService : IFoodAPIService
{
    private IHttpClientFactory _httpFactory;
    private IUserService _userService;
    private NavigationManager _navManager;
    public FoodAPIService(IHttpClientFactory httpFactory, IUserService userService, NavigationManager navigationManager)
    {
        _httpFactory = httpFactory;
        _userService = userService;
        _navManager = navigationManager;
    }
    public async Task CreateFood(FoodCreateDTO food)
    {
        var result = await (await GetClient()).PostAsJsonAsync("Food/", food);
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
    }

    public async Task DeleteFood(int id)
    {
        var result = await (await GetClient()).DeleteAsync($"Food/{id}");
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
    }

    public async Task<List<FoodGetDTOWithMeals>> FetchAllFoods()
    {
        var result = await (await GetClient()).GetAsync("Food/");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<List<FoodGetDTOWithMeals>>();
        }
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
        return [];
    }

    public async Task<FoodGetDTOWithMeals?> FetchFoodById(int id)
    {
        var result = await (await GetClient()).GetAsync($"Food/{id}");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<FoodGetDTOWithMeals>();
        }
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");
        return null;
    }

    public async Task UpdateFood(int id, FoodUpdateDTO newFood)
    {
        var result = await (await GetClient()).PutAsJsonAsync<FoodUpdateDTO>($"Food/{id}", newFood);
        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            _navManager.NavigateTo("/login");
        }
    }
    public async Task<HttpClient> GetClient()
    {

        var client = _httpFactory.CreateClient("API");
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
