using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ReadMe_Front.Models.Services
{
    public class CategoryService
    {
        private readonly CategoryEFRepo _efRepo;
        private readonly CategoryDapperRepo _dapperRepo;

        public CategoryService()
        {
            _efRepo = new CategoryEFRepo();
            _dapperRepo = new CategoryDapperRepo();
        }

        public List<GroupCategoryVm> GetProduct()
        {
            var products = _dapperRepo.GetProducts();

            List<GroupCategoryVm> productSet = products.GroupBy(c => c.ParentCategoriesName)
                                                         .Select(group => new GroupCategoryVm
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

            return productSet;
        }

        public List<GroupCategoryVm> GetGroupCat()
        {
            var categories = _dapperRepo.GetCategories();
            var categoryDictionary = categories.GroupBy(c => c.ParentCategoriesName)
                                               .ToDictionary(
                                                    group => group.Key, // Key 為 ParentCategoryName
                                                    group => group.Select(item => item.CategoryName).ToList() // Value 為其子 CategoryName 的列表
                                                );
            var groupCat = categoryDictionary.Select(d => new GroupCategoryVm
            {
                ParentCategoryName = d.Key,
                CategoryName = string.Join(", ", d.Value)
            }).ToList();

            return groupCat;
        }

        public List<GroupCategoryVm> GetParentCat(string input)
        {
            var parent = _efRepo.GetParent(input);
            if (parent == null)
            {
                throw new KeyNotFoundException("找不到此分類。");
            }

            var groupProduct = parent.GroupBy(p => p.CategoryName) // 按 CategoryName 分組
                                .Select(group => new GroupCategoryVm
                                {
                                    CategoryName = group.Key,
                                    ParentCategoryName = input,
                                    Products = group.Select(product => new GroupCategoryVm
                                    {
                                        Id = product.Id,
                                        Title = product.Title,
                                        Author = product.Author,
                                        Price = product.Price,
                                        ImageURL = product.ImageURL
                                    }).ToList()
                                }).ToList();
            return groupProduct;
        }

        public List<GroupCategoryVm> GetSubCat(string input)
        {
            var sub = _efRepo.GetSub(input);
            if (sub == null)
            {
                throw new KeyNotFoundException("找不到此分類。");
            }
            var groupProduct = sub.GroupBy(p => p.ParentCategoriesName) // 按 ParentCategoryName 分組
                                            .Select(group => new GroupCategoryVm
                                            {
                                                CategoryName = input,
                                                ParentCategoryName = group.Key,
                                                Products = group.Select(product => new GroupCategoryVm
                                                {
                                                    Id = product.Id,
                                                    Title = product.Title,
                                                    Author = product.Author,
                                                    Price = product.Price,
                                                    ImageURL = product.ImageURL
                                                }).ToList()
                                            }).OrderBy(x => x.Id).ToList();
            return groupProduct;
        }
    }
}