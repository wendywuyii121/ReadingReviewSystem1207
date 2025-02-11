using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using ReadingReviewSystem1207.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace ReadingReviewSystem1207.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile TeacherCertificate)
        {
            if (!ModelState.IsValid)
                return View(model);

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
                //❌ 原有檢查：學生註冊時必填學號，現已移除
                // if (string.IsNullOrWhiteSpace(model.StudentId))
                // {
                //     ModelState.AddModelError("StudentId", "學生註冊時必須填寫學號");
                //     return View(model);
                // }
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                StudentId = model.StudentId, // 學生代號現在為選填
                Role = assignedRole,
                Status = assignedStatus,
                TeacherCertificateUrl = null  // 確保初始化為 null
            };

            // 若為管理員，設定管理員驗證標記 (需在 ApplicationUser 中定義 IsAdminVerified 屬性)
            if (assignedRole == "Admin")
            {
                user.IsAdminVerified = true;
            }

            // 處理教師證上傳（僅在教師註冊且有上傳檔案時）
            if (model.IsTeacher && TeacherCertificate != null && TeacherCertificate.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "teacherCertificates");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(TeacherCertificate.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await TeacherCertificate.CopyToAsync(stream);
                }

                //➡️ 確保存入正確的 URL
                user.TeacherCertificateUrl = "/teacherCertificates/" + fileName;
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // 確保角色存在
                if (!await _roleManager.RoleExistsAsync(assignedRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(assignedRole));
                }

                await _userManager.AddToRoleAsync(user, assignedRole);

                if (assignedRole == "Teacher")
                {
                    TempData["Message"] = "您的教師身份註冊成功，請等待管理員審核。";
                    return RedirectToAction("Login", "Account");
                }
                else if (assignedRole == "Admin")
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
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
