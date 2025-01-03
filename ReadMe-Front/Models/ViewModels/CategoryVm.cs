using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReadMe_Front.Models.ViewModels
{
    public class CategoryVm
    {

        public List<GroupCategoryVm> GroupCategory { get; set; }

        public List<GroupCategoryVm> GroupProduct { get; set; }
    }

    public class GroupCategoryVm
    {
        public string CategoryName { get; set; }

        public string ParentCategoryName { get; set; }

        public List<GroupCategoryVm> Products { get; set; } = new List<GroupCategoryVm>();

        public int Id { get; set; }
    public class ProductVm
        public string Title { get; set; }

        public string Author { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        public int Price { get; set; }

        public string ImageURL { get; set; }

        public int Price { get; set; }

        public string ImageURL { get; set; }
        public int Price { get; set; }

        public string ImageURL { get; set; }
    }
}