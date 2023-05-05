using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RRshop.DTO;
using RRshop.Models;
using RRshop.Services;

System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (conf) => conf.LoginPath = "/Account/Login");
builder.Services.AddAuthorization();

builder.Services.AddDbContext<rrshopContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.Parse("8.0.32-mysql")));

builder.Services.AddAutoMapper(typeof(UserMapping));

builder.Services.AddSingleton<INotifyService,TgBot>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//Scaffold-DbContext "Server=localhost; Port=3306; Database=rrshop; Uid=root; Pwd=root" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -f

//dotnet aspnet-codegenerator controller -m Category -dc MyDbContext -rrshopContext CategoriesController -async