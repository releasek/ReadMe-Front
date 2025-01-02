using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadMe_Front.Models.ViewModels
{
    public class CategoryVm
    {

        public string CategoryName { get; set; }

        public string ParentCategoryName { get; set; }

        public List<ProductVm> Data { get; set; }

    }
    public class ProductVm
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public int Price { get; set; }

        public string ImageURL { get; set; }
    }
}