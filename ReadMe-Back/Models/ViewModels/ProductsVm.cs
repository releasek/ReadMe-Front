using System.ComponentModel.DataAnnotations.Schema;

namespace ReadMe_Back.Models.ViewModels
{
    public class ProductsVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }

        public int Price { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PublishDate { get; set; }

        public string ImageURL { get; set; }

        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
