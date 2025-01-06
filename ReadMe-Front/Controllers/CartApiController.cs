using ReadMe_Front.Models.Services;
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
}
