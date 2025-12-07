using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Client.Servicios;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.TipoProducto;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// CONFIGURACIÓN INICIAL
// =============================================
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(60);
});

// =============================================
//  CORS (NETLIFY + LOCALHOST)
// =============================================
var MyCors = "MyCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCors, policy =>
    {
        policy.WithOrigins(
                "https://onlyou2025.netlify.app", // tu front productivo
                "https://onlyou2.netlify.app",    // por si usas el segundo
                "http://127.0.0.1:5500",
                "http://localhost:5500",
                "http://localhost:4200",
                "http://localhost:5258"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();   // requerido si mandás tokens
    });
});

// =============================================
// SERVICIOS
// =============================================
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Repos, servicios, automapper
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

builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<IPagoService, PagoService>();


// =============================================
// APP
// =============================================
var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

// Routing
app.UseRouting();

// =============================================
//  Aplicar CORS ANTES de Auth
// =============================================
app.UseCors(MyCors);

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Cache
app.UseOutputCache();

// Endpoints
app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
