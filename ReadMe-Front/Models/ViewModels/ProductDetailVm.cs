using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.ViewModels
{
    public class ProductDetailVm
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }

        [Required]
        [StringLength(255)]
        public string Publisher { get; set; }

        public int Price { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int? PromotionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PublishDate { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageURL { get; set; }

        public string PromotionName { get; set; }

        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }

    }
}