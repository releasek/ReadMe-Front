using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryService _service;
        private CategoryDapperRepo _dapperRepo;
        private CategoryEFRepo _efRepo;

        public CategoriesController()
        {
            _service = new CategoryService();
            _dapperRepo = new CategoryDapperRepo();
            _efRepo = new CategoryEFRepo();
        }

        // GET: Categories
        public ActionResult Index(CategoryVm vm)
        {
            vm.GroupCategory = new CategoryService().GetGroupCat();
            vm.GroupProduct = new CategoryService().GetProduct();

            return View(vm);
        }

        public ActionResult ParentCategory(string parentCategoryName)
        {
            var vm = new CategoryVm
            {
                GroupProduct = new CategoryService().GetParentCat(parentCategoryName),
                GroupCategory = new CategoryService().GetGroupCat()
            };

            ViewBag.CurrentId = parentCategoryName;

            return View(vm);
        }

        public ActionResult SubCategory(string subCategoryName)
        {
            var vm = new CategoryVm
            {
                GroupProduct = new CategoryService().GetSubCat(subCategoryName),
                GroupCategory = new CategoryService().GetGroupCat()
            };

            ViewBag.CurrentId = subCategoryName;

            return View(vm);
        }
    }
}