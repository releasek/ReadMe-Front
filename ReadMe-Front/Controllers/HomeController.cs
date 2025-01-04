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
            var data = new ProductEFRepo().GetPromoteBooks();

			return View(data);
		}

        public ActionResult Promote()
        {
            // 永遠只查詢 promotionId = 1 的書籍
            var data = new ProductEFRepo().PromoteBooks(1);

            return View(data);
        }

        public ActionResult _NewBooksPartial()
        {
            var data = new ProductEFRepo().GetBooksByPublishdate();

            return View(data);
        }
        public ActionResult New()
        {
            var data = new ProductEFRepo().NewBooks();

            return View(data);
        }
        public ActionResult _TechBooksPartial()
        {
            var data = new ProductEFRepo().GetBooksByParentCategoryId();
            return View(data);
        }

        public ActionResult _ComfortBooksPartial()
        {
            var data = new ProductEFRepo().GetComfortBooks();

            return View(data);
        }
        public ActionResult Comfort()
        {
            var data = new ProductEFRepo().ComfortBooks();
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