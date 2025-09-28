
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

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(60);
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Servicio para el Client

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


// Coneccion con la BD / Context

builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));


// Identity


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();

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
            ClockSkew = TimeSpan.Zero // Esto reduce el sesgo del reloj a cero para fines de prueba
        };
    });



// Servicios

builder.Services.AddScoped<IImagenValidator, ImagenValidator>();
builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

//AUTOMAPPER

builder.Services.AddAutoMapper(typeof(Program));

// REPOSITORIOS

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




// =====================================================================================================================================


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirección HTTPS
app.UseHttpsRedirection();

// Archivos estáticos y Blazor
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

// Routing
app.UseRouting();

// Identity / Autenticación
app.UseAuthentication();
app.UseAuthorization();

// Output Cache
app.UseOutputCache();

// Mapeo de endpoints
app.MapControllers();        // ? Tus endpoints api/... deben ir antes del fallback
app.MapRazorPages();         // Razor pages
app.MapFallbackToFile("index.html"); // Blazor SPA fallback

app.Run();
