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
        [HttpDelete]
        [Route("{account}")]
        public IHttpActionResult GetCartInfo(string account)
        {
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
        [HttpDelete]
        [Route("remove-item")]
        public IHttpActionResult DeleteCartItem(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                return BadRequest("購物車項目編號錯誤");
            }
            _service.DeleteCartItem(cartItemId);
            return Ok($"成功刪除購物車項目{cartItemId}");
        }
        [HttpDelete]
        [Route("remove-cart")]
        public IHttpActionResult DeleteCart(int cartId)
        {
            if (cartId <= 0)
            {
                return BadRequest("購物車編號錯誤");
            }
            _service.DeleteCart(cartId);
            return Ok($"成功刪除購物車{cartId}");
        }

    }
}
