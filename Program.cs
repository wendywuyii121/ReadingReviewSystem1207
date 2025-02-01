using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models;

var builder = WebApplication.CreateBuilder(args);

// **註冊資料庫服務**
builder.Services.AddDbContext<ReadingReviewSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// **確保路由正確**
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
