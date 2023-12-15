using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using ProyectoProgra5.Data;
using ProyectoProgra5.Models;
using ProyectoGrupo5.Service;
using ProyectoGrupo5.Controllers;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de servicios
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<EmailServices>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuraci�n de la API
builder.Services.AddControllers();

// Configuraci�n de Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middleware de autorizaci�n y autenticaci�n (si es necesario)
app.UseAuthorization();
app.UseSession();

// Endpoint de la API
app.MapControllers();

// Configuraci�n de rutas para la aplicaci�n web
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
