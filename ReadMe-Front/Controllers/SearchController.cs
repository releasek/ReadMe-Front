using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Result(string input = "", int pageNumber = 1, int pageSize = 12)
        {
            var pagedResults = new CategoryEFRepo().Search(input, pageNumber, pageSize);
            var vm = new SearchVm
            {
                Data = pagedResults.Data,
                PaginationInfo = pagedResults.paginationInfo
            };

            // 將搜尋字串傳遞到 View
            ViewBag.SearchInput = input;
            return View(vm);
        }

        public ActionResult _PagedResult(string input, int pageNumber = 1, int pageSize = 12)
        {
            var pagedResults = new CategoryEFRepo().Search(input, pageNumber, pageSize);
            return PartialView("_PagedResult", pagedResults);
        }
    }
}