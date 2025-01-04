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
        //public CartVm GetCartInfo(string account)
        //{
        //    HttpResponseSubstitutionCallback==
        //}
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
        /// 取得會員購物車內商品
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<CartVm> GetCartByMemberProduct(string account)
        {
            using (var db = new AppDbContext())
            {
                var result = (from cart in db.Carts
                             join cartItem in db.CartItems
                             on cart.Id equals cartItem.CartId
                             join product in db.Products
                             on cartItem.ProductId equals product.Id
                             where cart.MemberAccount == account
                             select new CartVm
                             {
                                 Id = cart.Id,
                                 MemberAccount = cart.MemberAccount,
                                 ProductId = cartItem.ProductId,
                                 Price = cartItem.Price,
                                 Title = product.Title,
                                 Author = product.Author,
                                 Publisher = product.Publisher,
                                 ImageURL = product.ImageURL
                             }).ToList();
                return result;

            }
        }
    }
}