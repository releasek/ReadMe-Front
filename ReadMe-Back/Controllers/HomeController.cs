using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models;
using ReadMe_Back.Models.EFModels;
using System.Diagnostics;

namespace ReadMe_Back.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            //int currentUserId = 1; // admin
            //var rights = db.AdminUserRoleRels
            //    .Where(x => x.UserId == currentUserId)
            //    .SelectMany(ur => ur.Role.AdminRoleFunctionRels)
            //    .Select(rf => new { rf.FunctionId, rf.Function.FunctionName })
            //    .Distinct()
            //    .ToList();

            return View();
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
        public IActionResult OrderIndex()
        {
            return View();
        }
    }
}
