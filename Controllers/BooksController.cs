using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using System.Linq;

namespace ReadingReviewSystem1207.Controllers
{
    public class BooksController : Controller
    {
        private readonly ReadingReviewSystemDbContext _context;

        public BooksController(ReadingReviewSystemDbContext context)
        {
            _context = context;
        }

        // 顯示書籍列表
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }
        // 顯示新增書籍的表單 (GET: Books/Create)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 處理新增書籍的表單提交 (POST: Books/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile? coverImage)
        {
            if (ModelState.IsValid)
            {
                // 檢查是否有上傳圖片
                if (coverImage != null && coverImage.Length > 0)
                {
                    try
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(coverImage.FileName)}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverImage.CopyToAsync(stream);
                        }

                        book.CoverImageUrl = "/images/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"圖片上傳失敗: {ex.Message}");
                        ModelState.AddModelError("", "圖片上傳失敗，請重試。");
                        return View(book);
                    }
                }
                else
                {
                    // 未上傳圖片，設置默認圖片
                    book.CoverImageUrl = "/images/default-cover.jpg";
                }

                // 保存數據
                try
                {
                    _context.Books.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"數據保存失敗: {ex.Message}");
                    ModelState.AddModelError("", "無法保存數據，請重試。");
                }
            }

            return View(book);
        }

        // 顯示書籍詳情 (GET: Books/Details/{id})
        public IActionResult Details(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                Console.WriteLine($"找不到 id={id} 的書籍");
                return NotFound();
            }

            Console.WriteLine($"顯示書籍詳情: {book.Title}");
            return View(book);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                Console.WriteLine($"刪除失敗，找不到 ID 為 {id} 的書籍。");
                return NotFound();
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                Console.WriteLine($"成功刪除書籍: {book.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"刪除書籍失敗: {ex.Message}");
                return StatusCode(500, "無法刪除該書籍，請重試。");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
