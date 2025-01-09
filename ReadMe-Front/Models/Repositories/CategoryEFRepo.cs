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

        /// <summary>
        /// 篩選符合條件的商品
        /// </summary>
        /// <param name="input">篩選條件</param>
        /// <param name="pageNumber">第幾頁</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns></returns>
        public List<CategoryDto> Search(string input)
        {


            using (var db = new AppDbContext())
            {
                var results = from product in db.Products
                              join category in db.Categories
                              on product.CategoryId equals category.Id
                              join parentCategory in db.ParentCategories
                              on category.ParentCategoryId equals parentCategory.Id
                              where (product.Title.Contains(input) ||
                                     product.Author.Contains(input) ||
                                     product.Publisher.Contains(input))
                              select new CategoryDto
                              {
                                  Id = product.Id,
                                  Title = product.Title,
                                  Author = product.Author,
                                  Publisher = product.Publisher,
                                  Price = product.Price,
                                  ImageURL = product.ImageURL,
                                  CategoryName = category.CategoryName,
                                  ParentCategoriesName = parentCategory.ParentCategoriesName,
                              };

                return results.ToList();
            }
        }
    }
}