using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using System.IO;

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
            Console.WriteLine($"書籍數量: {books.Count}");
            return View(books);
        }

        // 顯示新增書籍的表單 (GET: Books/Create)
        public IActionResult Create()
        {
            try
            {
                var testFilePath = @"C:\Users\USER\source\repos\ReadingReviewSystem1207\wwwroot\images\test_file.txt";
                System.IO.File.WriteAllText(testFilePath, "測試寫入權限");
                Console.WriteLine("權限測試成功，文件已生成。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"權限測試失敗: {ex.Message}");
            }
            return View();
        }

        // 處理新增書籍的表單提交 (POST: Books/Create)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile coverImage)
        {
            Console.WriteLine("Create POST method triggered.");

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState is valid.");
                // 檢查是否有上傳圖片
                if (coverImage != null && coverImage.Length > 0)
                {
                    try
                    {
                        // 確保文件名唯一，避免重複覆蓋
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(coverImage.FileName)}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                        Console.WriteLine($"圖片將保存到: {filePath}");

                        // 確保目錄存在
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        // 保存圖片到指定路徑
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverImage.CopyToAsync(stream);
                        }

                        Console.WriteLine("圖片上傳成功");
                        // 將圖片的相對路徑保存到資料庫
                        book.CoverImageUrl = "/images/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        // 處理圖片上傳的異常情況
                        Console.WriteLine($"圖片上傳失敗: {ex.Message}");
                        ModelState.AddModelError("", "圖片上傳失敗，請重試。");
                        return View(book);
                    }
                }
                else
                {
                    // 如果未上傳圖片，設置默認值
                    book.CoverImageUrl = "/images/default-cover.jpg";
                }

                try
                {
                    // 保存其他書籍資料到資料庫
                    Console.WriteLine($"保存書籍: 書名={book.Title}, 心得={book.Review}, 圖片路徑={book.CoverImageUrl}");
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("書籍保存成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"書籍保存失敗: {ex.Message}");
                    ModelState.AddModelError("", "書籍保存失敗，請重試。");
                    return View(book);
                }

                return RedirectToAction(nameof(Index));
            }

            Console.WriteLine("ModelState is invalid.");
            // 打印 ModelState 錯誤信息
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            // 如果 ModelState 無效，返回表單頁面
            return View(book);
        }
    }
}
