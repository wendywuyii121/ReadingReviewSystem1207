using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReadingReviewSystem1207.Data;
using ReadingReviewSystem1207.Models.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class ReviewController : Controller
{
    private readonly AppDbContext _context;

    public ReviewController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Create - 顯示心得創建頁面，讓學生選擇教師
    public IActionResult Create()
    {
        var teachers = _context.AspNetUsers
                               .Where(u => u.Role == "Teacher")
                               .Select(u => new { u.Id, u.UserName }) // 確保取用的是 UserName
                               .ToList();

        ViewBag.TeacherList = new SelectList(teachers, "Id", "UserName");

        return View(new Review()); // 確保 Model 有值
    }

    // POST: Create - 學生提交心得，儲存 TeacherId
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Content,TeacherId")] Review review)
    {
        if (ModelState.IsValid)
        {
            // 取得當前登入學生的 ID
            review.StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            review.CreatedDate = DateTime.UtcNow; // 設定心得建立時間

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // 提交後返回心得列表
        }

        // 若表單驗證失敗，重新加載教師清單
        var teachers = _context.AspNetUsers
                               .Where(u => u.Role == "Teacher")
                               .Select(u => new { u.Id, u.UserName })
                               .ToList();
        ViewBag.TeacherList = new SelectList(teachers, "Id", "UserName");

        return View(review);
    }

    // GET: Index - 顯示所有心得（學生查看）
    public async Task<IActionResult> Index()
    {
        var reviews = await _context.Reviews
                                    .Include(r => r.Student)
                                    .Include(r => r.Teacher) // 確保加載教師資訊
                                    .ToListAsync();
        return View(reviews);
    }

    // GET: PendingReviews - 讓教師查看與自己相關的心得
    public async Task<IActionResult> PendingReviews()
    {
        var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier); // 取得當前登入教師的 ID

        var pendingReviews = await _context.Reviews
                                           .Include(r => r.Student) // 加載學生資訊
                                           .Where(r => r.TeacherId == teacherId) // 只篩選該教師的心得
                                           .ToListAsync();

        return View(pendingReviews);
    }
}
