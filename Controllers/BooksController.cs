using Microsoft.AspNetCore.Mvc;
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

        // 顯示書籍列表
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }
    }
}
