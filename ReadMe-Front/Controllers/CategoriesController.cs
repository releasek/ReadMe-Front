using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System.Collections.Generic;
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
            var categories = _repo.GetCategories();
            var products = _repo.GetProducts();

            var categoryDictionary = categories.GroupBy(c => c.ParentCategoriesName)
                                               .ToDictionary(
                                                    group => group.Key, // Key 為 ParentCategoryName
                                                    group => group.Select(item => item.CategoryName).ToList() // Value 為其子 CategoryName 的列表
                                                );

            var productSet = products.GroupBy(c => c.ParentCategoriesName);

            vm.GroupCategory = categoryDictionary.Select(d => new GroupCategoryVm
            {
                ParentCategoryName = d.Key,
                CategoryName = string.Join(", ", d.Value)
            }).ToList();

            List<GroupCategoryVm> groupProduct = productSet.Select(group => new GroupCategoryVm
            {
                ParentCategoryName = group.Key,  // 父類別名稱
                Products = group.Select(product => new GroupCategoryVm
                {
                    Id = product.Id,
                    Title = product.Title,
                    Author = product.Author,
                    Price = product.Price,
                    ImageURL = product.ImageURL
                }).ToList()
            }).ToList();

            vm.GroupProduct = groupProduct;

            return View(vm);
        }
    }
}