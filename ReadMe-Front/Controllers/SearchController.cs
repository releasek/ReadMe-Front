using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.Infra;
using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class SearchController : Controller
    {
        //private SearchService _searchService;
        private SearchEFRepo _efRepo;

        public SearchController()
        {
            _efRepo = new SearchEFRepo();
        }

        // GET: Search
        public ActionResult Result()
        {
            return View();
        }
        public ActionResult _Products(string keyword, int pageNumber = 1, int pageSize = 12, string selectedOption = null)
        {
            ViewBag.keyword = keyword;

            var pagedProducts = _efRepo.Search(keyword, pageNumber, pageSize, selectedOption);

            // 從完整的資料集中計算每個分類的數量（不受分頁影響）
            var allProducts = _efRepo.SearchWithoutPagination(keyword); // 不分頁的完整資料
            var categoryCounts = allProducts
                .GroupBy(x => x.CategoryName) // 按 CategoryName 分組
                .ToDictionary(g => g.Key, g => g.Count()); // 計算每組的數量並轉為字典

            // 將搜尋總數數量傳入 ViewBag
            ViewBag.TotalCounts = allProducts.Count();

            // 將分類數量傳入 ViewBag
            ViewBag.CategoryCounts = categoryCounts;

            var options = pagedProducts.Data
                        .Select(x => x.CategoryName)
                        .Distinct()
                        .OrderBy(x => x)
                        .ToList();

            var model = new Paged<SearchVm>(pagedProducts.Data.Select(dto => dto.Dto2Vm()).ToList(), pagedProducts.Pagination, selectedOption, options);

            return PartialView(model);
        }
    }
}