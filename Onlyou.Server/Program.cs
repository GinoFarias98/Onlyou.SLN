using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Client.Autorizacion;
using Onlyou.Client.Servicios;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(60);
});

// =====================================
// CORS
// =====================================
var MyCors = "MyCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCors, policy =>
    {
        policy.WithOrigins(
            "https://onlyou2025.netlify.app",
            "https://onlyou2.netlify.app"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Razor & Blazor
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// DB
builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Servicios y repositorios
builder.Services.AddScoped<IImagenValidator, ImagenValidator>();
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
builder.Services.AddScoped<IRepositorioProveedor, RepositorioProveedor>();
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();
builder.Services.AddScoped<IRepositorioMarca, RepositorioMarca>();
builder.Services.AddScoped<IRepositorioColor, RepositorioColor>();
builder.Services.AddScoped<IRepositorioTalle, RepositorioTalle>();
builder.Services.AddScoped<IRepositorioEstadoPedido, RepositorioEstadoPedido>();
builder.Services.AddScoped<IRepositorioTipoProducto, RepositorioTipoProducto>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();

builder.Services.AddScoped<IRepositorioMovimiento, RepositorioMovimiento>();
builder.Services.AddScoped<IRepositorioTipoMovimiento, RepositorioTipoMovimiento>();
builder.Services.AddScoped<IRepositorioPago, RepositorioPago>();
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
builder.Services.AddScoped<IRepositorioPedidoItem, RepositorioPedidoItem>();
builder.Services.AddScoped<IRepositorioCaja, RepositorioCaja>();
builder.Services.AddScoped<IRepositorioObservacionCaja, RepositorioObservacionCaja>();
builder.Services.AddScoped<IRepositorioTipoPago, RepositorioTipoPago>();
builder.Services.AddScoped<IRepositorioObservacionPago, RepositorioObservacionPago>();
builder.Services.AddScoped<IRepositorioObservacionPedido, RepositorioObservacionPedido>();


builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<IPagoService, PagoService>();





var app = builder.Build();

// =====================================
// Pipeline
// =====================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Archivos para Blazor
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

app.UseRouting();

app.UseCors(MyCors);

app.UseAuthentication();
app.UseAuthorization();

// =====================================
// RUTA PARA TU WEB CLIENTE HTML
// =====================================
app.MapGet("/sitioweb", async context =>
{
    await context.Response.SendFileAsync("wwwroot/sitioweb/index.html");
});

// Controllers & Razor
app.MapControllers();
app.MapRazorPages();

// Fallback Blazor
app.MapFallbackToFile("index.html");

// Cache
app.UseOutputCache();

app.Run();
