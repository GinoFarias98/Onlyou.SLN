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

// Configuración ANTES de los servicios
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(60);
});

// =====================================================================================
//  AGREGADO: REGISTRO DE CORS PARA NETLIFY (NO SE TOCÓ NADA MÁS)
// =====================================================================================
var MyCors = "MyCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyCors, policy =>
    {
        policy.WithOrigins("https://onlyou2025.netlify.app",
                           "https://onlyou2.netlify.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// =====================================================================================


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Servicio para el Client
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Conexión con la BD / Context
builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
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

// Servicios
builder.Services.AddScoped<IImagenValidator, ImagenValidator>();
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Repositorios
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

// Servicios
builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<IPagoService, PagoService>();

// =====================================================================================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Archivos estáticos y Blazor
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

// Routing
app.UseRouting();

// =====================================================================================
//  AGREGADO: ACTIVAR CORS (ANTES DE AUTH) – NO ROMPE NADA
// =====================================================================================
app.UseCors(MyCors);
// =====================================================================================

// CORS que ya tenías (NO SE TOCÓ)
app.UseCors(policy => policy
    .WithOrigins("http://127.0.0.1:5500",
                 "http://localhost:5500",
                 "http://localhost:5258",
                 "http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

// Identity / Autenticación
app.UseAuthentication();
app.UseAuthorization();

// Output Cache
app.UseOutputCache();

// Mapeo de endpoints
app.MapControllers();
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();