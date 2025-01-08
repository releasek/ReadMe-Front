using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
    public class OrderVm
    {
        public int Id { get; set; }

        public string OrderName { get; set; }

        public int UserID { get; set; }

        public int TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

    }
}