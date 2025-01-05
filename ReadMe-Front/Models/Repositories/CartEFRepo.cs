using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class CartEFRepo
    {
        /// <summary>
        /// 取得會員帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Cart GetCartByMemberAccount(string account)
        {
            using (var db = new AppDbContext())
            {
                return db.Carts.FirstOrDefault(c => c.MemberAccount == account);
            }
        }
        /// <summary>
        /// 取得使用者購物車資料
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public CartVm GetCartInfo(string account)
        {
            using (var db = new AppDbContext())
            {
                var cart = db.Carts.FirstOrDefault(c => c.MemberAccount == account);
                if (cart == null)
                {
                    cart = new Cart { MemberAccount = account };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
                var cartItems = db.CartItems.Where(ci => ci.CartId == cart.Id)
                    .Select(ci => new CartItemVm
                    {
                        Id = ci.Id,
                        Cartid = ci.CartId,
                        ProductId = ci.ProductId,
                        Price = ci.Price,
                        Title = ci.Product.Title,
                        Author = ci.Product.Author,
                        Publisher = ci.Product.Publisher,
                        ImageURL = ci.Product.ImageURL
                    }).ToList();
                var cartVm = new CartVm
                {
                    Id = cart.Id,
                    MemberAccount = cart.MemberAccount,
                    CartItems = cartItems,
                };
                return cartVm;
            }
        }
            /// <summary>
            /// 新增購物車
            /// </summary>
            /// <param name="cartItem"></param>
            public void AddCartItem(int cartId, int productId, int Price)
        {
            using(var db = new AppDbContext())
            {
                var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Price = Price;
                }
                else
                {
                    var newItem = new CartItem { CartId = cartId, ProductId = productId, Price = Price };
                    db.CartItems.Add(newItem);
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 刪除購物車項目
        /// </summary>
        /// <param name="cartItemId"></param>
        public void DeleteCartItem(int cartItemId)
        {
            using (var db = new AppDbContext())
            {
                var cartItem = db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem != null)
                {
                    db.CartItems.Remove(cartItem);
                    db.SaveChanges();
                }
            }
        }
        /// <summary>
        /// 刪除購物車
        /// </summary>
        /// <param name="cartId"></param>
        public void DeleteCart(int cartId)
        {
            using (var db = new AppDbContext())
            {
                var cart = db.Carts.FirstOrDefault(c => c.Id == cartId);
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
            }
        }

    }
}