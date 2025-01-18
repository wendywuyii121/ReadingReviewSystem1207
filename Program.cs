using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models;

var builder = WebApplication.CreateBuilder(args);

// ���U��Ʈw�A�ȡA�ϥ� SQL Server �ñq appsettings.json ��Ū���s�u�r��
builder.Services.AddDbContext<ReadingReviewSystemDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// �]�w��ƫO�@�K�_���x�s��m
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Users\USER\AppData\Local\ASP.NET\DataProtection-Keys"));

// �K�[ MVC �䴩
builder.Services.AddControllersWithViews();

var app = builder.Build();

// HTTP �ШD�޹D�]�m
if (!app.Environment.IsDevelopment())
{
    // ���O�}�o���ҮɡA�ϥΦ۩w�q���~����
    app.UseExceptionHandler("/Home/Error");
    // �ҥ� HSTS�]HTTP �Y��ǿ�w���^
    app.UseHsts();
}

// �j�� HTTPS�A�N HTTP �ШD���w�V�� HTTPS
app.UseHttpsRedirection();
// ���\�R�A�ɮס]�p�Ϥ��BCSS�BJS ���^
app.UseStaticFiles();
// �ҥθ���
app.UseRouting();
// �ҥα��v
app.UseAuthorization();

// �]�w�q�{���ѡA����� Books�A�ʧ@�� Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();
