using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingReviewSystem1207.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<BooksController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // **🔹 Index 方法 (顯示用戶的心得) 🔹**
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 記錄進入 Index 方法
            _logger.LogInformation("Debug: 進入 Books Index 方法");
            Console.WriteLine("Books Index 方法開始執行");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("用戶未登入，導向至 Login 頁面");
                return RedirectToAction("Login", "Account");
            }

            var books = await _context.Books
                .Where(b => b.OwnerId == user.Id)
                .ToListAsync();

            _logger.LogInformation("成功取得用戶 {UserId} 的書籍數量: {BookCount}", user.Id, books.Count);

            return View(books); // **✅ 確保返回 View**
        }

        // **🔹 Create 方法 (顯示表單) 🔹**
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Debug: 進入 Books Create 方法 (GET)");
            Console.WriteLine("Books Create 方法開始執行 (GET)");
            return View();
        }

        // **🔹 Create 方法 (處理表單提交) 🔹**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile? coverImage)
        {
            _logger.LogInformation("Debug: 進入 Books Create 方法 (POST)");
            Console.WriteLine("Books Create 方法開始執行 (POST)");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("表單驗證失敗");
                return View(book);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("用戶未登入，導向至 Login 頁面");
                return RedirectToAction("Login", "Account");
            }

            // 處理封面圖片上傳
            if (coverImage != null && coverImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", coverImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(stream);
                }
                book.CoverImagePath = "/images/" + coverImage.FileName;
            }

            book.OwnerId = user.Id;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("成功新增書籍 {BookTitle} (ID: {BookId})", book.Title, book.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
