using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Net;
using System.Web.Http;

namespace ReadMe_Front.Controllers
{
    public class CartApiController : ApiController
    {
        private readonly CartService _service;

        public CartApiController()
        {
            _service = new CartService();
        }
        /// <summary>
        /// 取得購物車資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cartapi/getAllCartItems")]
        public IHttpActionResult GetCartInfo()
        {
            string account = User.Identity.Name;

            if (string.IsNullOrEmpty(account))
            {
                return BadRequest("帳號為空");

            }
            var cart = _service.GetCartInfo(account);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpGet]
        [Route("api/cartapi/getPromotions")]
        public IHttpActionResult GetPromotions()
        {
            var promotions = _service.GetPromotionsVmItem();
            return Ok(promotions);
        }

        //https://localhost:44395/api/cartapi?cartItemId=3
        [HttpDelete]
        [Route("api/cartapi/deleteCartIrem")]
        public IHttpActionResult DeleteCartItem(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                return BadRequest("購物車項目編號錯誤");
            }
            _service.DeleteCartItem(cartItemId);
            return Ok($"成功刪除購物車項目{cartItemId}");
        }
        [HttpPost]
        [Route("api/cartapi/addToFavorite")]
        public IHttpActionResult AddFavoriteItem( int productid)
        {
            string account = User.Identity.Name;
            var userid = _service.GetUserId(account);
            try
            {
                _service.AddFavorite(userid, productid);
                return Ok(new { Message = "商品已成功加入收藏清單" });
            }
            catch (Exception ex)
            {
                return BadRequest($"加入收藏清單失敗：{ex.Message}");
            }
        }
        [HttpPost]
        [Route("api/cartapi/addCart")]
        public IHttpActionResult AddCartItem([FromBody] AddCartVm dto)
        {
            if (dto == null || dto.ProductId <= 0 || dto.Price <= 0)
            {
                return BadRequest("請提供有效的商品資訊！");
            }

            string account = "user06";


            try
            {
                // 呼叫服務層新增購物車項目
                _service.AddCartItem(account, dto.ProductId, dto.Price);
                return Ok(new { message = "成功加入購物車" });
            }
            catch (Exception ex)
            {
                // 返回錯誤訊息
                return Content(HttpStatusCode.InternalServerError, new { message = $"加入購物車失敗：{ex.Message}" });
            }
        }

        //[HttpDelete]
        //[Route("api/cartapi/clearCart")]
        //public IHttpActionResult ClearCart()
        //{
        //    string account = User.Identity.Name;

        //    if (string.IsNullOrEmpty(account))
        //    {
        //        return BadRequest("帳號為空");
        //    }

        //    _service.ClearCart(account);
        //    return Ok("購物車已清空");
        //}

    }
    public class AddCartVm
    {
        public int ProductId { get; set; }
        public int Price { get; set; }
    }
}
