using Blazored.LocalStorage;
using DomPizza.App.Services;
using DomPizza.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7193/") });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
