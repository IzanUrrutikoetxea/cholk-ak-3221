using Blazored.LocalStorage;
using cholk_ak_3221;
using cholk_ak_3221.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ── API HttpClient ────────────────────────────────────────────────────────────
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7100";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });

// ── Core services ─────────────────────────────────────────────────────────────
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSingleton<AppStateService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IDataStore, ApiDataStore>();

await builder.Build().RunAsync();
