using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Onlyou.Client;
using Onlyou.Client.Autorizacion;
using Onlyou.Client.Servicios;
using Onlyou.Client.Servicios.ServiciosEntidades;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<IHttpServicios, HttpServicios>();
builder.Services.AddScoped<ICategoriaServicios, CategoriaServicios>();
builder.Services.AddScoped(typeof(FiltroGenericoServicio<>));


builder.Services.AddScoped<ProveedorAutenticacionJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, ProveedorAutenticacionJWT>(prov => prov.GetRequiredService<ProveedorAutenticacionJWT>());
builder.Services.AddScoped<ILoginService, ProveedorAutenticacionJWT>(prov => prov.GetRequiredService<ProveedorAutenticacionJWT>());


builder.Services.AddSweetAlert2();


await builder.Build().RunAsync();
