using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using ReadingReviewSystem1207.Models; // 替換成你的專案命名空間
using ReadingReviewSystem1207.Data; // 確保這行引入資料庫上下文

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context; // ✅ **新增 _context 變數**

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) // ✅ **正確的建構函數**
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context)); // ✅ **正確初始化 _context**
    }

    public IActionResult Index()
    {
        return View();
    }

    // 1️⃣ 顯示待審核教師頁面
    public IActionResult PendingTeachers()
    {
        var teachers = _context.Users
            .Where(u => u.Role == "Teacher" && u.Status == "Pending")
            .ToList();

        // ★ 修改處：檢查 TeacherCertificateUrl 是否正確讀取
        // 例如，若需要在檢視頁面中顯示教師證檔案連結，可直接存取 teacher.TeacherCertificateUrl 屬性
        // 測試用：var sampleCertificateUrl = teachers.FirstOrDefault()?.TeacherCertificateUrl;

        return View(teachers);
    }

    // 2️⃣ 批准教師申請
    [HttpPost]
    public IActionResult ApproveTeacher(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return BadRequest("User ID is required");

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return NotFound("User not found");

        user.Status = "Approved";
        _context.SaveChanges(); // ✅ **確保變更被存入資料庫**

        return RedirectToAction("PendingTeachers"); // 批准後回到待審核列表
    }

    // 3️⃣ 拒絕教師申請
    [HttpPost]
    public IActionResult RejectTeacher(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return BadRequest("User ID is required");

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return NotFound("User not found");

        user.Status = "Rejected";
        _context.SaveChanges(); // ✅ **確保變更被存入資料庫**

        return RedirectToAction("PendingTeachers"); // 拒絕後回到待審核列表
    }

    // 4️⃣ 顯示已批准教師頁面
    public IActionResult ApprovedTeachers()
    {
        var approvedTeachers = _context.Users
            .Where(u => u.Role == "Teacher" && u.Status == "Approved")
            .ToList();

        // ★ 修改處：檢查 TeacherCertificateUrl 是否正確讀取
        // 同樣可透過 approvedTeachers 中的各筆記錄存取 TeacherCertificateUrl 屬性
        // 測試用：var sampleCertificateUrl = approvedTeachers.FirstOrDefault()?.TeacherCertificateUrl;

        return View(approvedTeachers);
    }
}
