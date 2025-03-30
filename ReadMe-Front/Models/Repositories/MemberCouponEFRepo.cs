using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class MemberCouponEFRepo
    {
        private readonly AppDbContext _db;
        public MemberCouponEFRepo()
        {
            _db = new AppDbContext();
        }
        public List<Member_Coupons> GetMemberCoupon()
        {
            return _db.member_coupons.ToList();
        }
        public void Create(Member_Coupons memberCoupon)
        {
            if (memberCoupon == null)
            {
                throw new ArgumentNullException(nameof(memberCoupon));
            }
            _db.member_coupons.Add(memberCoupon);
            _db.SaveChanges();
        }
    }
}