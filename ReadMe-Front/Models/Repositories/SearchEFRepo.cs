using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Infra;
using System.Collections.Generic;
using System.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class SearchEFRepo
    {
        public Paged<CategoryDto> Search(string input, int pageNumber, int pageSize, string selectedOption)
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

                // 若 selectedOption 不為 null，加入額外篩選條件
                if (!string.IsNullOrEmpty(selectedOption))
                {
                    query = query.Where(p => p.Category.CategoryName == selectedOption);
                }


                // 計算總筆數
                var totalCount = query.Count();
                var pagination = new Pagination(pageNumber, pageSize, totalCount, selectedOption);


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

                var options = data
                            .Select(x => x.CategoryName)
                            .Distinct()
                            .OrderBy(x => x)
                            .ToList();

                return new Paged<CategoryDto>(data, pagination, selectedOption, options);
            }
        }

        public List<CategoryDto> SearchWithoutPagination(string keyword)
        {
            keyword = keyword?.Trim();

            using (var db = new AppDbContext())
            {
                var query = db.Products
                    .Include("Categories")
                    .Where(p =>
                        (string.IsNullOrEmpty(keyword) ||
                         p.Title.ToLower().Contains(keyword.ToLower()) ||
                         p.Author.ToLower().Contains(keyword.ToLower()) ||
                         p.Publisher.ToLower().Contains(keyword.ToLower()))
                    );

                return query.Select(p => new CategoryDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageURL = p.ImageURL,
                    Price = p.Price,
                    Publisher = p.Publisher,
                    Author = p.Author,
                    CategoryName = p.Category.CategoryName,
                }).ToList();
            }
        }
    }
}