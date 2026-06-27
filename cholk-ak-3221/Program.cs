using Blazored.LocalStorage;
using cholk_ak_3221;
using cholk_ak_3221.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<AppStateService>();
builder.Services.AddScoped<DataStore>();

await builder.Build().RunAsync();
