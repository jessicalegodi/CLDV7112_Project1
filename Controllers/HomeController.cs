using Microsoft.AspNetCore.Mvc;

namespace CLDV7112_Project1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
