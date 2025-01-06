using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Description;

namespace ReadMe_Front.Controllers
{
    public class CartDeleteApiController : ApiController
    {
        private readonly CartService _service;
        public CartDeleteApiController()
        {
            _service = new CartService();
        }

        [HttpDelete]
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
