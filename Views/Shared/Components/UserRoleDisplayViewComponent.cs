using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReadingReviewSystem1207.ViewComponents
{
    public class UserRoleDisplayViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleDisplayViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Content(""); // 沒登入就不顯示
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Content(""); // 找不到使用者就不顯示
            }

            var roles = await _userManager.GetRolesAsync(user);
            return View(roles);
        }
    }
}
