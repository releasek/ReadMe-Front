using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Security;

namespace ReadMe_Back.Controllers
{
    [FunctionAuthorize("訂單管理")]
    public class OrderSearchController : Controller
    {
        private readonly AppDbContext _context;

        public OrderSearchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OrderMain()
        {
            return View();
        }


    }
}
