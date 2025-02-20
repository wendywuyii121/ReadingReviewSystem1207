using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Data;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Models.Entities;

public class ReviewController : Controller
{
    private readonly AppDbContext _context;

    public ReviewController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int classId)
    {
        var reviews = await _context.Reviews
        .Include(r => r.Student)
        .ThenInclude(s => s.Class)
        .ToListAsync();
        return View(reviews);
    }
}
