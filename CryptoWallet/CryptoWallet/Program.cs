using CryptoWallet.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

/*
 
 SI LLEGA A FALLAR POR PROBLEMAS DE CONEXION (criptoya), CONFIGURO TIMEOUT .  
  
  builder.Services.AddHttpClient("default", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

*/

// configuro CORS para permitir solicitudes desde el frontend (Vue)
 

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirVue", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// El requerimiento HTTP de pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCors("PermitirVue");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
