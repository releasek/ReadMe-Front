using ReadMe_Back.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReadMe_Back.Models.ViewModels
{
    public class ProductVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "書名是必填項。")]
        public string Title { get; set; }

        [Required(ErrorMessage = "作者是必填項。")]
        public string Author { get; set; }

        [Required(ErrorMessage = "出版社是必填項。")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "價格是必填項。")]
        [Range(0, int.MaxValue, ErrorMessage = "價格必須大於等於 0")]
        public int Price { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PublishDate { get; set; }

        [Required(ErrorMessage = "圖片檔名是必填項。")]
        public string ImageURL { get; set; }

        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
    }

    public class ProductIndexVm
    {
        public List<ProductVm> Products { get; set; } // 所有產品

        public List<CategoryDto> AllCategories { get; set; } // 所有分類
    }
}
