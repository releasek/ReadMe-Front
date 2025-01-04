using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.Services;
using ReadMe_Front.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var categories = _dapperRepo.GetCategories();
            var products = _dapperRepo.GetProducts();

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

        public ActionResult ParentCategory(string parentCategoryName)
        {
            if (parentCategoryName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var parent = _efRepo.GetCategoryBooks(parentCategoryName);
            var groupProduct = parent.GroupBy(p => p.CategoryName) // 按 CategoryName 分組
                                .Select(group => new GroupCategoryVm
                                {
                                    CategoryName = group.Key,
                                    ParentCategoryName = parentCategoryName,
                                    Products = group.Select(product => new GroupCategoryVm
                                    {
                                        Id = product.Id,
                                        Title = product.Title,
                                        Author = product.Author,
                                        Price = product.Price,
                                        ImageURL = product.ImageURL
                                    }).ToList()
                                }).ToList();

            var vm = new CategoryVm
            {
                GroupProduct = groupProduct
            };

            var categories = _dapperRepo.GetCategories();
            var categoryDictionary = categories.GroupBy(c => c.ParentCategoriesName)
                                               .ToDictionary(
                                                    group => group.Key, // Key 為 ParentCategoryName
                                                    group => group.Select(item => item.CategoryName).ToList() // Value 為其子 CategoryName 的列表
                                                );


            vm.GroupCategory = categoryDictionary.Select(d => new GroupCategoryVm
            {
                ParentCategoryName = d.Key,
                CategoryName = string.Join(", ", d.Value)
            }).ToList();

            ViewBag.CurrentId = parentCategoryName;

            return View(vm);
        }
    }
}