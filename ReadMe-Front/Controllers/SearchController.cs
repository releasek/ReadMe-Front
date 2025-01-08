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
        public ActionResult _Products(string keyword, int pageNumber = 1, int pageSize = 12)
        {
            ViewBag.keyword = keyword;

            var pagedProducts = _efRepo.Search(keyword, pageNumber, pageSize);
            var model = new Paged<SearchVm>(pagedProducts.Data.Select(dto => dto.Dto2Vm()).ToList(), pagedProducts.Pagination);
            return PartialView(model);
        }
    }
}