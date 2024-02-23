using ClothesShop.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option=>option.IdleTimeout = TimeSpan.FromHours(2));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//DBcontext
builder.Services.AddDbContext<ClothingShopContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DBConn")
    )
);

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

app.UseAuthorization();
app.UseSession();

// กำหนดเส้นทาง routing / ? = null c# ทำได้
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
