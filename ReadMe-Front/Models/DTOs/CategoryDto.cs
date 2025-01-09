using ReadMe_Front.Models.ViewModels;

namespace ReadMe_Front.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public int Price { get; set; }

        public string ImageURL { get; set; }

        public string CategoryName { get; set; }

        public string ParentCategoriesName { get; set; }
    }

    public static class ProductExts
    {
        public static SearchVm Dto2Vm(this CategoryDto dto)
        {
            return new SearchVm
            {
                Id = dto.Id,
                Title = dto.Title,
                Price = dto.Price,
                ImageURL = dto.ImageURL,
                Author = dto.Author,
                Publisher = dto.Publisher,
                Category = dto.CategoryName,
                ParentCategory = dto.ParentCategoriesName,
            };
        }
    }
}