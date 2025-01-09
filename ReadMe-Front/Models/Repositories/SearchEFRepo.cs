using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using System.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class SearchEFRepo
    {
        public Paged<CategoryDto> Search(string input, int pageNumber, int pageSize)
        {
            // 確保 input 不為 null，並移除首尾多餘空白
            input = input?.Trim();
            using (var db = new AppDbContext())
            {
                // 查詢符合條件的書籍
                var query = db.Products
                        .Include("Categories") // 加載 Category 資料
                        .Include("Categories.ParentCategories") // 加載 ParentCategory 資料
                        .Where(p =>
                            (string.IsNullOrEmpty(input) || // 若無 input，則不篩選
                             p.Title.ToLower().Contains(input.ToLower()) ||    // 書名包含 input
                             p.Author.ToLower().Contains(input.ToLower()) ||   // 作者包含 input
                             p.Publisher.ToLower().Contains(input.ToLower()))); // 出版社包含 input


                // 計算總筆數
                var totalCount = query.Count();
                var pagination = new Pagination(pageNumber, pageSize, totalCount);


                // 分頁資料
                var data = query
                        .OrderBy(p => p.Id) // 順序很重要，保證穩定分頁
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(p => new CategoryDto
                        {
                            Id = p.Id,
                            Title = p.Title,
                            ImageURL = p.ImageURL,
                            Price = p.Price,
                            Publisher = p.Publisher,
                            Author = p.Author,
                            CategoryName = p.Category.CategoryName,
                            ParentCategoriesName = p.Category.ParentCategory.ParentCategoriesName
                        }).ToList();

                return new Paged<CategoryDto>(data, pagination);
            }
        }

    }
}