using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models;

namespace ReadingReviewSystem1207.Controllers
{
    public class BooksController : Controller
    {
        private readonly ReadingReviewSystemDbContext _context;

        public BooksController(ReadingReviewSystemDbContext context)
        {
            _context = context;
        }

        // GET: Books/Index
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile? coverImage)
        {
            if (ModelState.IsValid)
            {
                if (coverImage != null && coverImage.Length > 0)
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
                else
                {
                    book.CoverImageUrl = "/images/default-cover.jpg"; // 預設封面圖
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile? coverImage)
        {
            if (id != book.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // 從資料庫中取得原始記錄，避免覆蓋掉未更新的資料
                    var existingBook = await _context.Books.FindAsync(id);
                    if (existingBook == null)
                        return NotFound();

                    // 更新基本欄位
                    existingBook.Title = book.Title;
                    existingBook.Review = book.Review;

                    // 如果有上傳新的封面圖片則更新，否則保留原值
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(coverImage.FileName)}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverImage.CopyToAsync(stream);
                        }
                        existingBook.CoverImageUrl = "/images/" + uniqueFileName;
                    }

                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
