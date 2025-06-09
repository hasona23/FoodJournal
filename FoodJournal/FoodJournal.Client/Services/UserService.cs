using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Json;

namespace FoodJournal.Client.Services;

public class UserService : IUserService
{

    private IHttpClientFactory _httpClientFactory;
    private IJSRuntime _jsRuntime;

    private NavigationManager _navManager;
    public UserService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _httpClientFactory = httpClientFactory;
        _jsRuntime = jsRuntime;
        _navManager = navigationManager;
    }
    public async Task<string> Login(string email, string password)
    {
        var result = await GetClient().PostAsJsonAsync("User/login", new { email, password });
        if (result.IsSuccessStatusCode)
        {
            Token token = await result.Content.ReadFromJsonAsync<Token>();
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "authToken", token.AccessToken);
            return "";
        }
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            return "Wrong Email or Password";
        return "Error Happened";
    }

    public async Task Logout()
    {
        await GetClient().PostAsJsonAsync("User/logout", new { });
    }

    public async Task<string> Register(string email, string password)
    {
        var result = await GetClient().PostAsJsonAsync("User/register", new { email, password });
        if (result.IsSuccessStatusCode)
        {

            return "";
        }
        if (result.StatusCode == HttpStatusCode.BadRequest)
            return "Email Already Taken";
        return "Error Happened";
    }
    public async Task<string> GetToken()
    {
        return await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");
    }

    public async Task<UserInfo> GetUserInfoAsync()
    {
        try
        {
            var client = GetClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetToken());
            var result = await client.GetAsync("/api/User/manage/info");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<UserInfo>();
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
                _navManager.NavigateTo("/login");

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public UserInfo GetUserInfo()
    {
        var result = GetClient().GetAsync("/api/User/manage/info").Result;
        if (result.IsSuccessStatusCode)
        {
            return result.Content.ReadFromJsonAsync<UserInfo>().Result;
        }
        if (result.StatusCode == HttpStatusCode.Unauthorized)
            _navManager.NavigateTo("/login");

        return null;
    }

    public HttpClient GetClient()
    {

        return _httpClientFactory.CreateClient("API");

    }
    public class Token
    {
        public string AccessToken { get; set; }
    }
    public class UserInfo
    {
        public string Email { get; set; }
    }
}
