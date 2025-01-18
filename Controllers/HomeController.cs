using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReadingReviewSystem1207.Models;

namespace ReadingReviewSystem1207.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // �ק� Index ��k�A���w�V�� Books/Index
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Books");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
