using Microsoft.AspNetCore.Mvc;

namespace ReadMe_Back.Controllers
{
    public class PromotionsController : Controller
    {
        public IActionResult Promotions()
        {
            return View();
        }
    }
}
