using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryService _service;
        private CategoryDapperRepo _repo;

        public CategoriesController()
        {
            _service = new CategoryService();
            _repo = new CategoryDapperRepo();
        }

        // GET: Categories
        public ActionResult Index(CategoryVm vm)
        {
            var dto = _repo.GetAll();

            var groupedCategories = dto.GroupBy(x => new { x.CategoryName, x.ParentCategoriesName })
                                       .Select(group => new CategoryVm
                                       {
                                           CategoryName = group.Key.CategoryName,
                                           ParentCategoryName = group.Key.ParentCategoriesName,
                                           Data = group.Select(product => new ProductVm
                                           {
                                               Id = product.Id,
                                               Title = product.Title,
                                               Author = product.Author,
                                               Price = product.Price,
                                               ImageURL = product.ImageURL
                                           }).ToList()

                                       }).ToList();

            return View(groupedCategories);

        }
    }
}