using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class MemberOrderController : Controller
    {

        [Authorize]
        public ActionResult MemberOrderDetails()
        {
            return View();
        }
        [Authorize]
        public ActionResult MemberProductDetail()
        {
            // 從查詢字串中取得 orderName 的值
            string orderName = Request.QueryString["orderName"];

            // 檢查是否取得有效的 orderName
            if (string.IsNullOrEmpty(orderName))
            {
                ViewBag.ErrorMessage = "無法取得訂單名稱，請確認 URL 是否正確。";
                return View("Error"); // 顯示錯誤頁面
            }

            // 將 orderName 傳遞到 View
            ViewBag.OrderName = orderName;

            return View();
        }

    }
}