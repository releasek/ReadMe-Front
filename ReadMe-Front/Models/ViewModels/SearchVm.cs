namespace ReadMe_Front.Models.ViewModels
{
    public class SearchVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ParentCategory { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
    }

}