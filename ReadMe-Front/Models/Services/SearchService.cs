using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Repositories;
using System.Linq;

namespace ReadMe_Front.Models.Services
{
    public class SearchService
    {
        private readonly CategoryEFRepo _efRepo;

        public SearchService()
        {
            _efRepo = new CategoryEFRepo();
        }


        public Paged<CategoryDto> FilterResults(string input, int pageNumber = 1, int pageSize = 12, string categoryName = null, string publisher = null, string author = null)
        {
            // 獲取所有符合搜尋條件的資料
            var allResults = _efRepo.Search(input);

            // 再篩選邏輯
            var filteredResults = allResults.AsQueryable();

            // 根據分類篩選
            if (!string.IsNullOrEmpty(categoryName))
            {
                filteredResults = filteredResults.Where(x => x.CategoryName == categoryName);
            }

            // 根據出版社篩選
            if (!string.IsNullOrEmpty(publisher))
            {
                filteredResults = filteredResults.Where(x => x.Publisher == publisher);
            }

            // 根據作者篩選
            if (!string.IsNullOrEmpty(author))
            {
                filteredResults = filteredResults.Where(x => x.Author == author);
            }

            var filteredList = filteredResults.ToList();

            int recordCount = filteredList.Count;

            // 執行分頁
            var pagedData = filteredList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new Paged<CategoryDto>(pagedData, pageNumber, pageSize, recordCount);
        }

    }
}