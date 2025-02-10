using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using ReadingReviewSystem1207.ViewModels;

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

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                StudentId = model.StudentId,
                Role = model.IsTeacher ? "Teacher" : "Student",
                Status = model.IsTeacher ? "Pending" : "Approved"
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.IsTeacher)
                {
                    TempData["Message"] = "您的教師身份註冊成功，請等待管理員審核。";
                    return RedirectToAction("Login", "Account"); // ✅ 設定 TempData 並跳轉到登入
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

            // ✅ 使用 SignInManager 進行登入，避免 No sign-in authentication handler 錯誤
            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

            return RedirectToAction("Index", "Home");
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
