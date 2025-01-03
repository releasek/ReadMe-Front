using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadMe_Front.Models.ViewModels
{
    public class CategoryVm
    {

        public string CategoryName { get; set; }

        public string ParentCategoryName { get; set; }

        public List<ProductDetailVm> Data { get; set; }

    }
}