using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Repositories
{
    public class ProductEFRepo
    {
        public List<Product> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return db.Products.ToList();
            }
        }
        /// <summary>
        /// 每項商品的詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductDetailVm> GetById(int id)
        {
            using (var db = new AppDbContext())
            {
               var result = from product in db.Products
                            join category in db.Categories
                            on product.CategoryId equals category.Id
                            join parentCategory in db.ParentCategories
                            on category.ParentCategoryId equals parentCategory.Id
                            join promotion in db.Promotions
                            on product.PromotionId equals promotion.Id into promotionGroup
                            from promo in promotionGroup.DefaultIfEmpty() // PromotionId 可能為 null
                            where product.Id == id
                            select new ProductDetailVm
                            {
                                Id = product.Id,
                                Title = product.Title,
                                Author = product.Author,
                                Publisher = product.Publisher,
                                Price = product.Price,
                                Description = product.Description,
                                CategoryId = product.CategoryId,
                                CategoryName = category.CategoryName,
                                ParentCategoryName = parentCategory.ParentCategoriesName, // 不需要判斷 null
                                PromotionId = product.PromotionId,
                                PromotionName = promo != null ? promo.PromotionName : string.Empty, // 預設空字串
                                PublishDate = product.PublishDate,
                                ImageURL = product.ImageURL
                            };
                return result.ToList();

            }
            
        }


    }
}