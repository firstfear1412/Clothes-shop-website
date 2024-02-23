using Microsoft.AspNetCore.Mvc;

namespace ClothesShop.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dev()
        {
            return View();
        }
    }
}
