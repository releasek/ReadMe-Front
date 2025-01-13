using Microsoft.VisualBasic.ApplicationServices;
using ReadMe_Front.Models.DAOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class CartEFRepo
    {
        public List<OrderVm> MemberOrderDetail(string orderName)
        {
            using (var db = new AppDbContext())
            {

                var result = from OrderDetails in db.OrderDetails
                             join Orders in db.Orders on OrderDetails.OrderId equals Orders.Id
                             join Products in db.Products on OrderDetails.ProductId equals Products.Id
                             where Orders.OrderName == orderName
                             select new OrderVm
                             {
                                 Id = Orders.Id,
                                 UnitPrice = OrderDetails.UnitPrice,
                                 TotalAmount = Orders.TotalAmount,
                                 Titel = Products.Title,
                                 Author = Products.Author,
                                 image= Products.ImageURL,
                             };
                return result.ToList();
            }
        }
        /// <summary>
        /// 會員訂單資料
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<OrderVm> MemberOrder(string account)
        {
            using (var db = new AppDbContext())
            {
                var userId = GetUserid(account);
                
                return db.Orders.Where(a=>a.UserID==userId)
                .AsEnumerable() // 將數據加載到記憶體
                .Select(o => new OrderVm
                {
                    Id = o.Id,
                    OrderName = o.OrderName,
                    UserID = o.UserID,
                    TotalAmount = o.TotalAmount,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd"), // 格式化日期
                    Payment ="信用卡",
                    PaymentStatus = "已付款",
                }).ToList();
            }
        }
        /// <summary>
        /// 單筆訂單資料
        /// </summary>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public OrderVm GetOrderDetail(string orderName)
        {
            using (var db = new AppDbContext())
            {
                var order = db.Orders.Where(n => n.OrderName == orderName)
                    .Select(o => new OrderVm
                    {
                        Id = o.Id,
                        OrderName = o.OrderName,
                        UserID = o.UserID,
                        TotalAmount = o.TotalAmount,
                        
                        
                    }).FirstOrDefault();
                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with name '{orderName}' was not found.");
                }
                return order;
            }
        }
        /// <summary>
        /// 創建訂單
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cartid"></param>
        public OrderVm CreateOrder(string account, int cartid)
        {
            using (var db = new AppDbContext())
            {
                var _service = new CartService();
                var cart = _service.GetCartInfo(account);
                var userid= db.Users.FirstOrDefault(u => u.Account == account).Id;
                var orderName = $"{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
                var order = new Order
                {
                    OrderName = orderName,
                    UserID = userid,
                    TotalAmount =cart.DiscountedPrice,
                    OrderDate = DateTime.Now
                };
                foreach (var item in cart.CartItems)
                {
                    var orderItem = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId =item.ProductId,
                        UnitPrice = item.Price,
                        Quantity = 1
                    };
                    db.OrderDetails.Add(orderItem);
                }
                db.Orders.Add(order);
                db.SaveChanges();

                return db.Orders.Where(o => o.OrderName == order.OrderName)
                    .Select(o => new OrderVm
                    {
                        Id = o.Id,
                        OrderName = o.OrderName,
                        UserID = o.UserID,
                        TotalAmount = o.TotalAmount,                      
                    }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 取得促銷活動
        /// </summary>
        /// <returns></returns>
        public List<PromotionVm> GetPromotionsVmItem()
        {
            using (var db = new AppDbContext())
            {
                return db.Promotions.Select(p => new PromotionVm
                {
                    Id = p.Id,
                    PromotionName = p.PromotionName,
                    DiscountValue = p.DiscountValue,
                    MinPurchase = p.MinPurchase,
                    ValidFrom = p.ValidFrom,
                    ValidTo = p.ValidTo
                }).ToList();
            }
        }
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
                    TotalPrice = cartItems.Sum(item => item.Price)
                };
                return cartVm;
            }
        }
        /// <summary>
        /// 新增購物車
        /// </summary>
        /// <param name="cartItem"></param>
        public void AddCartItem(string account, int productId, int price)
        {
            using (var db = new AppDbContext())
            {
                // 取得或建立購物車
                var cart = db.Carts.FirstOrDefault(u => u.MemberAccount == account);
                if (cart == null)
                {
                    cart = new Cart { MemberAccount = account };
                    db.Carts.Add(cart);
                    db.SaveChanges(); // 保存以取得新建的 Cart.Id
                }

                // 嘗試取得購物車項目
                var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

                if (cartItem != null)
                {
                    // 更新價格
                    cartItem.Price = price;
                }
                else
                {
                    // 新增購物車項目
                    var newCartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = productId,
                        Price = price
                    };
                    db.CartItems.Add(newCartItem);
                }

                // 保存更改
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
                var cartItem = db.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemId);
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
        /// <summary>
        /// 加入收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="productid"></param>
        public void AddFavorite(int userid, int productid)
        {
            using (var db = new AppDbContext())
            {
                // 檢查是否已存在
                var favorite = db.Wishlists.FirstOrDefault(u => u.UserId == userid && u.ProductId == productid);

                // 當不存在時新增
                if (favorite == null)
                {
                    var newfavorite = new Wishlist { UserId = userid, ProductId = productid };
                    db.Wishlists.Add(newfavorite);
                    db.SaveChanges(); // 儲存變更
                }
            }
        }
        /// <summary>
        /// 透過帳號得到userId
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetUserid(string name)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Account == name);
                if (user != null)
                {
                    return user.Id; // 回傳使用者 ID
                }
                else
                {
                    throw new Exception($"找不到名稱為 {name} 的使用者。"); // 找不到則拋出例外
                }
            }
        }

    }
}