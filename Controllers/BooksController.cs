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

        // **🔹 Index 方法 (顯示用戶的書籍心得) 🔹**
        [HttpGet]
        public async Task<IActionResult> Index()
        {
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
            return View(books);
        }

        // **🔹 Create 方法 (顯示新增表單) 🔹**
        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation("Debug: 進入 Books Create 方法 (GET)");
            Console.WriteLine("Books Create 方法開始執行 (GET)");
            return View();
        }

        // **🔹 Create 方法 (處理新增請求) 🔹**
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Debug: 進入 Books Details 方法 | 書籍ID: {BookId}", id);
            Console.WriteLine("Books Details 方法開始執行");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == user.Id);
            if (book == null)
            {
                _logger.LogWarning("未找到書籍 (ID: {BookId}) 或沒有權限", id);
                return NotFound();
            }

            return View(book);
        }


        // **🔹 Edit 方法 (顯示編輯表單) 🔹**
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Debug: 進入 Books Edit 方法 (GET) | 書籍ID: {BookId}", id);
            Console.WriteLine("Books Edit 方法開始執行 (GET)");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == user.Id);
            if (book == null)
            {
                _logger.LogWarning("未找到書籍 (ID: {BookId}) 或沒有權限", id);
                return NotFound();
            }

            return View(book);
        }

        // **🔹 Edit 方法 (處理編輯請求) 🔹**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile? coverImage)
        {
            _logger.LogInformation("Debug: 進入 Books Edit 方法 (POST) | 書籍ID: {BookId}", id);
            Console.WriteLine("Books Edit 方法開始執行 (POST)");

            if (id != book.Id)
            {
                _logger.LogWarning("ID 不匹配 (表單 ID: {FormId}, URL ID: {UrlId})", book.Id, id);
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == user.Id);
            if (existingBook == null)
            {
                _logger.LogWarning("未找到書籍 (ID: {BookId}) 或沒有權限", id);
                return NotFound();
            }

            existingBook.Title = book.Title;
            existingBook.Review = book.Review;

            // 更新封面圖片
            if (coverImage != null && coverImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", coverImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(stream);
                }
                existingBook.CoverImagePath = "/images/" + coverImage.FileName;
            }

            _context.Update(existingBook);
            await _context.SaveChangesAsync();

            _logger.LogInformation("成功編輯書籍 {BookTitle} (ID: {BookId})", book.Title, book.Id);
            return RedirectToAction(nameof(Index));
        }

        // **🔹 Delete 方法 (刪除書籍) 🔹**
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Debug: 進入 Books Delete 方法 | 書籍ID: {BookId}", id);
            Console.WriteLine("Books Delete 方法開始執行");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && b.OwnerId == user.Id);
            if (book == null)
            {
                _logger.LogWarning("未找到書籍 (ID: {BookId}) 或沒有權限", id);
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("成功刪除書籍 (ID: {BookId})", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
