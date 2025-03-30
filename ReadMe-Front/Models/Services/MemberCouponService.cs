using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Services
{
    public class MemberCouponService
    {
        private readonly MemberCouponEFRepo _memberCouponRepo;
        public MemberCouponService()
        {
            _memberCouponRepo = new MemberCouponEFRepo();
        }

        public List<Member_Coupons> GetMemberCoupon()
        {
            return _memberCouponRepo.GetMemberCoupon();
        }
        public void AddMemberCoupon(Member_Coupons memberCoupon)
        {
            if (memberCoupon == null)
            {
                throw new ArgumentNullException(nameof(memberCoupon));
            }
            _memberCouponRepo.Create(memberCoupon);
        }

    }
}