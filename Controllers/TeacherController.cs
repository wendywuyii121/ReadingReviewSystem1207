using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
