using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
    public class CartVm
    {
        public int Id { get; set; }
        public string MemberAccount { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<CartItemVm> CartItems { get; set; }

        public int DiscountedPrice { get; set; }

        public string PromotionName { get; set; }

        public int TotalPrice { get; set; }
        public bool AllowCheckout => CartItems.Any(); //至少有一項商品才能結帳
    }
}