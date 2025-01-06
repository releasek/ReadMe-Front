using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Services
{
    public class CartService
    {
        private readonly CartEFRepo _cartEFRepo;
        
        public CartService()
        {
            _cartEFRepo = new CartEFRepo();
        }
        public List<PromotionVm> GetPromotionsVmItem()
        {
            return _cartEFRepo.GetPromotionsVmItem();
        }
        public CartVm GetCartInfo(string account)
        {
            var cart = _cartEFRepo.GetCartInfo(account);
            var discount = _cartEFRepo.GetPromotionsVmItem();
            int  totalPrice = cart.TotalPrice;

            // 找到适用的优惠券
            var applicablePromotion = discount
                 .Where(p => totalPrice >= p.MinPurchase)
                 .OrderByDescending(p => p.MinPurchase)
                 .FirstOrDefault();

            int discountAmount = 0;
            string discountDescription = null;

            if (applicablePromotion != null)
            {
                discountAmount = applicablePromotion.DiscountValue;
                discountDescription = applicablePromotion.PromotionName;
            }

            int finalPrice = totalPrice - discountAmount;

            // 设置购物车的折扣信息
            var cartVm = new CartVm
            {
                Id = cart.Id,
                MemberAccount = cart.MemberAccount,
                CartItems = cart.CartItems,
                DiscountedPrice = finalPrice,
                PromotionName = discountDescription,
                TotalPrice = totalPrice
            };

            return cart;


        }
        public void AddCartItem(int cartId, int productId, int price)
        {
            _cartEFRepo.AddCartItem(cartId,productId,price);
        }
        public void DeleteCartItem(int cartItemId)
        {
            _cartEFRepo.DeleteCartItem(cartItemId);
        }
        public void DeleteCart(int cartId)
        {
            _cartEFRepo.DeleteCart(cartId);
        }
    }
}