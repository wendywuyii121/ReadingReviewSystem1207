using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using ReadingReviewSystem1207.ViewModels;
using System.IO; // 確保加入 System.IO

namespace ReadingReviewSystem1207.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 註冊 GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 註冊 POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ➡️ 新增：宣告 assignedRole 與 assignedStatus 變數
            string assignedRole;
            string assignedStatus;

            if (!string.IsNullOrEmpty(model.AdminCode) && model.AdminCode == "410149")
            {
                assignedRole = "Admin";
                assignedStatus = "Approved";
            }
            else if (model.IsTeacher)
            {
                assignedRole = "Teacher";
                assignedStatus = "Pending";
            }
            else
            {
                assignedRole = "Student";
                assignedStatus = "Approved";
            }

            // ★ 修改：新增教師證上傳邏輯，僅對 Teacher 角色進行
            string teacherCertificatePath = null;
            if (assignedRole == "Teacher" && model.TeacherCertificate != null && model.TeacherCertificate.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "teacherCertificates");
                Directory.CreateDirectory(uploadsFolder); // 確保目錄存在

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.TeacherCertificate.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.TeacherCertificate.CopyToAsync(stream);
                }

                teacherCertificatePath = "/teacherCertificates/" + uniqueFileName; // 儲存相對路徑
            }
            // ★ 修改結束

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                StudentId = model.StudentId,
                Role = assignedRole,
                Status = assignedStatus,
                TeacherCertificateUrl = teacherCertificatePath // ★ 修改：確保教師證路徑存入資料庫
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (assignedRole == "Teacher")
                {
                    TempData["Message"] = "您的教師身份註冊成功，請等待管理員審核。";
                    return RedirectToAction("Login", "Account"); // 跳轉到登入頁面
                }
                else if (assignedRole == "Admin")
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Admin"); // 管理員註冊成功後跳轉至 /Admin/Index
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home"); // 學生註冊成功後跳轉至首頁
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // 登入 GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 登入 POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError(string.Empty, "登入失敗，請檢查帳號密碼。");
                return View(model);
            }

            if (user.Role == "Teacher" && user.Status == "Pending")
            {
                TempData["Message"] = "您好，管理員審核中，請於 1-3 個工作天後登入。";
                return RedirectToAction("Login", "Account");
            }

            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

            // 新增：判斷如果使用者是管理員則跳轉到 /Admin/Index
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // 登出
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // 存取被拒
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
