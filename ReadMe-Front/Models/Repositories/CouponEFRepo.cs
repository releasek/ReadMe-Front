using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class CouponEFRepo
    {
        private readonly AppDbContext _db;
        public CouponEFRepo()
        {
            _db = new AppDbContext();
        }
        public async Task<List<Coupon>> GetCoupon()
        {
            return await _db.coupons.ToListAsync();
        }
        public async Task Create(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new ArgumentNullException(nameof(coupon));
            }
            _db.coupons.Add(coupon);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var coupon = await _db.coupons.FindAsync(id);
            if (coupon == null)
            {
                throw new KeyNotFoundException("找不到促銷活動");
            }
            _db.coupons.Remove(coupon);
            await _db.SaveChangesAsync();
        }
        public async Task Update(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new ArgumentNullException(nameof(coupon));
            }
            _db.Entry(coupon).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}