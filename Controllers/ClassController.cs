using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace ReadingReviewSystem1207.Controllers
{
    public class ClassController : Controller
    {
        private readonly AppDbContext _context;

        public ClassController(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 顯示教師的班級列表
        public async Task<IActionResult> Index()
        {
            var classes = await _context.SchoolClasses.ToListAsync();
            return View(classes);
        }
    }
}
