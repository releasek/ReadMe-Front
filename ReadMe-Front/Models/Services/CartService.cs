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
        public CartVm GetCartInfo(string account)
        {
            return _cartEFRepo.GetCartInfo(account);
        }
        public void AddCartItem(int cartId, int productId, int price)
        {
            _cartEFRepo.AddCartItem(cartId,productId,price);
        }
    }
}