using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models;

var builder = WebApplication.CreateBuilder(args);

// 註冊資料庫服務，使用 SQL Server 並從 appsettings.json 中讀取連線字串
builder.Services.AddDbContext<ReadingReviewSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 設定資料保護密鑰的儲存位置
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\USER\AppData\Local\ASP.NET\DataProtection-Keys"));

// 添加 MVC 支援
builder.Services.AddControllersWithViews();

var app = builder.Build();

// HTTP 請求管道設置
if (!app.Environment.IsDevelopment())
{
    // 不是開發環境時，使用自定義錯誤頁面
    app.UseExceptionHandler("/Home/Error");
    // 啟用 HSTS（HTTP 嚴格傳輸安全）
    app.UseHsts();
}

// 強制 HTTPS，將 HTTP 請求重定向至 HTTPS
app.UseHttpsRedirection();
// 允許靜態檔案（如圖片、CSS、JS 等）
app.UseStaticFiles();
// 啟用路由
app.UseRouting();
// 啟用授權
app.UseAuthorization();

// 設定默認路由，控制器為 Books，動作為 Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
