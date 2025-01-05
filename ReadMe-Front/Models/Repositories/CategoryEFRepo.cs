using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using System.Collections.Generic;
using System.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class CategoryEFRepo
    {
        public List<CategoryDto> GetParent(string input)
        {
            using (var db = new AppDbContext())
            {
                var result = from product in db.Products
                             join category in db.Categories
                             on product.CategoryId equals category.Id
                             join parentCategory in db.ParentCategories
                             on category.ParentCategoryId equals parentCategory.Id
                             where parentCategory.ParentCategoriesName == input
                             select new CategoryDto
                             {
                                 Id = product.Id,
                                 Title = product.Title,
                                 Author = product.Author,
                                 Price = product.Price,
                                 CategoryName = category.CategoryName,
                                 ParentCategoriesName = input,
                                 ImageURL = product.ImageURL
                             };
                return result.ToList();
            }
        }

        public List<CategoryDto> GetSub(string input)
        {
            using (var db = new AppDbContext())
            {
                var result = from product in db.Products
                             join category in db.Categories
                             on product.CategoryId equals category.Id
                             join parentCategory in db.ParentCategories
                             on category.ParentCategoryId equals parentCategory.Id
                             where category.CategoryName == input
                             select new CategoryDto
                             {
                                 Id = product.Id,
                                 Title = product.Title,
                                 Author = product.Author,
                                 Price = product.Price,
                                 CategoryName = input,
                                 ParentCategoriesName = parentCategory.ParentCategoriesName,
                                 ImageURL = product.ImageURL
                             };
                return result.ToList();
            }
        }


    }
}