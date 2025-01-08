using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class SearchController : Controller
    {
        private SearchService _searchService;
        private CategoryEFRepo _efRepo;

        public SearchController()
        {
            _searchService = new SearchService();
            _efRepo = new CategoryEFRepo();
        }

        // GET: Search
        //public ActionResult Result(string input = "", int pageNumber = 1, int pageSize = 12)
        //{
        //    var initialResults = new SearchVm
        //    {
        //        Data = new SearchService().Search(input, pageNumber, pageSize).Data,
        //        PaginationInfo = new SearchService().Search(input, pageNumber, pageSize).paginationInfo
        //    };

        //    // 將搜尋字串傳遞到 View
        //    ViewBag.SearchInput = input;
        //    return View(initialResults);
        //}

        public ActionResult Result()
        {
            return View();
        }

        public ActionResult _PagedResult(string input = "", int pageNumber = 1, int pageSize = 12, string categoryName = null, string publisher = null, string author = null)
        {
            var pagedResults = _searchService.FilterResults(input, pageNumber, pageSize, categoryName, publisher, author);

            ViewBag.SearchInput = input;

            return View(pagedResults);
        }

        [HttpPost]
        public JsonResult FilterResults(string input, int pageNumber = 1, int pageSize = 12, string categoryName = null, string publisher = null, string author = null)
        {
            var filterResults = _searchService.FilterResults(input);

            // 返回 JSON 結果
            return Json(new
            {
                data = filterResults.Data,
                pagination = filterResults.PaginationInfo
            });
        }

    }
}