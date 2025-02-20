using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models;

var builder = WebApplication.CreateBuilder(args);

// �]�w ApplicationDbContext (�Ω� Identity)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// �]�w AppDbContext (�Ω��L��ƪ�A�Ҧp Class�BReview)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// �]�w Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ��e Identity �K�X�F��
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

builder.Services.AddControllersWithViews();

//  ���\�}�o���Ҫ� Cookie �b HTTP �U�ǿ�
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.MinimumSameSitePolicy = SameSiteMode.Lax; // �קK Cookie ����
        options.Secure = CookieSecurePolicy.None; // ���\ Cookie �b HTTP �ǿ�
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.None; // ���\�������� Cookie �b HTTP �U�ǿ�
        options.Cookie.SameSite = SameSiteMode.Lax; // ��e SameSite ����
    });
}

var app = builder.Build();

// �]�w�}�o�P�������Ҫ����~�B�z
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// �ҥ� Cookie �]�w
app.UseCookiePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// �]�w����
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
