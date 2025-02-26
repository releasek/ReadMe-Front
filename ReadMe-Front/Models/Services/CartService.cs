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
        public OrderVm GetOrderDetail(string orderName)
        {
            return _cartEFRepo.GetOrderDetail(orderName);
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

            // 找到適合優惠卷
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

            // 設置購物車折扣
            var cartItem = new CartVm
            {
                Id = cart.Id,
                MemberAccount = cart.MemberAccount,
                CartItems = cart.CartItems,
                DiscountedPrice = finalPrice,
                PromotionName = discountDescription,
                TotalPrice = totalPrice,
                DiscountValue = applicablePromotion?.DiscountValue ?? 0 // 如果 applicablePromotion 為 null，則設為 0
            };

            return cartItem;


        }
        public List<OrderVm> GetMemberOrderDetail(string orderName)
        {
            return _cartEFRepo.MemberOrderDetail(orderName);
        }
        public List<OrderVm> GetMemberOrder(string account)
        {
            return _cartEFRepo.MemberOrder(account);
        }
        public OrderVm CreateOrder(string account, int cartid)
        {
            return _cartEFRepo.CreateOrder(account, cartid);
        }
        public int GetUserId(string account)
        {
            var userid = _cartEFRepo.GetUserid(account);
            return userid;
        }
        public void AddFavorite(int userid, int productId)
        {
            _cartEFRepo.AddFavorite(userid, productId);
        }
        public void AddCartItem(string account, int productId, int price)
        {
            _cartEFRepo.AddCartItem(account,productId,price);
        }
        public void DeleteCartItem(int cartItemId)
        {
            _cartEFRepo.DeleteCartItem(cartItemId);
        }
        public void ClearCart(int cartId)
        {
            _cartEFRepo.DeleteCart(cartId);
        }
    }
}