using FoodJournal.Client;
using FoodJournal.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new("http://localhost:5043/api/");
});
builder.Services.AddScoped<IMealAPIService, MealAPIService>();
builder.Services.AddScoped<IFoodAPIService, FoodAPIService>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();
