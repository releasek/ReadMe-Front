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

        public int Price { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }

        public string ImageURL { get; set; }
    }
}