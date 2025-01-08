using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ReadMe_Front.Controllers
{

    public class CartController : Controller
    {
        private readonly CartService _service;

        public CartController()
        {
            _service = new CartService();
        }
        [Authorize]
        // GET: Cart
        public ActionResult Cart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToFavorite(int productId)
        {
            string account = User.Identity.Name; // 從身份中獲取用戶帳號
            var userId = _service.GetUserId(account);

            try
            {
                _service.AddFavorite(userId, productId);
                TempData["Message"] = "商品已成功加入收藏清單！";
                //return RedirectToAction("Products", "Cart");
                return RedirectToAction("Favorite", "Products");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"加入收藏清單失敗：{ex.Message}";
                return RedirectToAction("Details", "Products", new { id = productId });
            }
        }

        // 加入購物車
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int price)
        {
            string account = User.Identity.Name; // 從身份中獲取用戶帳號
            try
            {
                // 添加到購物車的業務邏輯
                _service.AddCartItem(account,productId,price);
                TempData["Message"] = "商品已成功加入購物車！";
                return RedirectToAction("Cart", "Cart"); // 跳轉到購物車頁面
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"加入購物車失敗：{ex.Message}";
                return RedirectToAction("Details", "Products", new { id = productId }); // 返回商品詳情頁
            }
        }

    }
}