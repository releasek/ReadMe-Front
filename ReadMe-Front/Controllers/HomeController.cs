using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult _PromotePartial()
		{
            var data = new ProductEFRepo().GetBooks();

			return View(data);
		}




		public ActionResult Login()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}