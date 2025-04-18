using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReadMe_Front.Controllers.Api
{
   
    public class CouponApiController : ApiController
    {
        private readonly CouponService _couponService;
        private readonly MemberCouponService _memberCouponService;
        public CouponApiController()
        {
            _couponService = new CouponService();
            _memberCouponService = new MemberCouponService();
        }
        [HttpGet]
        [Route("api/coupon")]
        public async Task<IHttpActionResult> GetAllCoupon()
        {
            var coupons = await _couponService.GetCoupon();
            return Ok(coupons);
        }
        [HttpPost]
        [Route("api/membercoupon")]
        public IHttpActionResult AddMemberCoupon([FromBody] Member_Coupons memberCoupon)
        {
			string account = User.Identity.Name;           
            if (memberCoupon == null)
            {
                return BadRequest("請輸入優惠券");
            }
            try 
            { 
                _memberCouponService.AddMemberCoupon(memberCoupon);
                return Ok(new {message="優惠券新增成功" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("發生錯誤，請聯繫系統管理員", ex));
            }
        }


    }
}
