using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models;
using System.Linq;

public class TeacherController : Controller
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        ViewBag.TeacherName = "王老師"; // 假設這是測試數據
        return View();
    }

    public IActionResult PendingReviews()
    {
        var pendingReviews = _context.Reviews.Where(r => !r.IsReviewed).ToList();
        return View(pendingReviews);
    }

    public IActionResult Reviewed()
    {
        var reviewed = _context.Reviews.Where(r => r.IsReviewed).ToList();
        return View(reviewed);
    }
}
