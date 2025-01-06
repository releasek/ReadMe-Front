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
}