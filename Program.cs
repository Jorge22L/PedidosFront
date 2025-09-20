using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PedidosFront;
using PedidosFront.Services;
using System.Diagnostics;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Leer configuración desde appsettings.json


var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];

// Registrar HttpClient con la url de la api
builder.Services.AddScoped(sp => new HttpClient
{

    BaseAddress = new Uri(baseUrl!)
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PedidoService>();

builder.RootComponents.Add<HeadOutlet>("head::after");


await builder.Build().RunAsync();
