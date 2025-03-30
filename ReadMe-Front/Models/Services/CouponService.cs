using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadMe_Front.Models.Services
{
    public class CouponService
    {
        private readonly CouponEFRepo _couponRepo;
        public CouponService()
        {
            _couponRepo = new CouponEFRepo();
        }

        public async Task<List<Coupon>> GetCoupon()
        {
            return await _couponRepo.GetCoupon();
        }
        public async Task Create(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new System.ArgumentNullException(nameof(coupon));
            }
            await _couponRepo.Create(coupon);
        }
        public async Task Delete(int id)
        {
            await _couponRepo.Delete(id);
        }
        public async Task Update(Coupon coupon)
        {
            if (coupon == null)
            {
                throw new System.ArgumentNullException(nameof(coupon));
            }
            await _couponRepo.Update(coupon);
        }
    }
}