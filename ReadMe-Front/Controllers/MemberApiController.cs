using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReadMe_Front.Controllers
{
    public class MemberApiController : ApiController
    {
        private readonly CartService _service;
        public MemberApiController()
        {
            _service = new CartService();
        }
        [HttpGet]
        [Route("api/memberapi/getMemberOrder")]
        public IHttpActionResult GetMemberOrder()
        {
            string account = User.Identity.Name;
            //string account = "user06";

            if (string.IsNullOrEmpty(account))
            {
                return BadRequest("帳號為空");
            }

            var orders = _service.GetMemberOrder(account);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
        [HttpGet]
        [Route("api/memberapi/getMemberOrderDetail")]
        public IHttpActionResult GetMemberOrderDetail(string orderName)
        {
            string account = "suer06";

            if (string.IsNullOrEmpty(account))
            {
                return BadRequest("帳號為空");
            }
            var orders = _service.GetMemberOrderDetail( orderName);
            return Ok(orders);
        }
    }
}