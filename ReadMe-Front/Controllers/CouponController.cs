using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class CouponController : Controller
    {
        // GET: Coupon
        public ActionResult Coupon()
        {
            return View();
        }
        public ActionResult MemberCoupon()
        {
            return View();
        }
    }
}