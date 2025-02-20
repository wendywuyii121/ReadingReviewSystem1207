using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models;

var builder = WebApplication.CreateBuilder(args);

// 設定 ApplicationDbContext (用於 Identity)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 設定 AppDbContext (用於其他資料表，例如 Class、Review)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 設定 Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 放寬 Identity 密碼政策
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

builder.Services.AddControllersWithViews();

//  允許開發環境的 Cookie 在 HTTP 下傳輸
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.Lax; // 避免 Cookie 限制
        options.Secure = CookieSecurePolicy.None; // 允許 Cookie 在 HTTP 傳輸
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // 允許身份驗證 Cookie 在 HTTP 下傳輸
        options.Cookie.SameSite = SameSiteMode.Lax; // 放寬 SameSite 限制
    });
}

var app = builder.Build();

// 設定開發與正式環境的錯誤處理
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// 啟用 Cookie 設定
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 設定路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
