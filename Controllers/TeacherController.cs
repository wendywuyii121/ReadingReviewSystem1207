using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Data;

[Route("Teacher")]
public class TeacherController : Controller
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        ViewBag.TeacherName = "王老師";
        return View();
    }

    // ✅ 明確指定路由，避免找不到
    [HttpGet("PendingReviews")]
    public IActionResult PendingReviews()
    {
        var pendingReviews = _context.Reviews.Where(r => !r.IsReviewed).ToList();
        return View(pendingReviews);
    }
}
