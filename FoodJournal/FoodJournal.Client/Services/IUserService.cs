using FoodJournal.Client.Models;

namespace FoodJournal.Client.Services;

public interface IUserService
{
    public Task<string> Login(string email, string password);
    public Task Logout();
    public Task<string> Register(string email, string password);
    public Task<string> GetToken();
    public Task<UserInfo> GetUserInfo();
}
