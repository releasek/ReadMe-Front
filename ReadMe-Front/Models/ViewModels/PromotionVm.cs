using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
    public class PromotionVm
    {
        public int Id { get; set; }

        public string PromotionName { get; set; }

        public int DiscountValue { get; set; }

        public int MinPurchase { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

   
    }
}