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
        public ProductDetailVm GetById(int id)
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
                return result.FirstOrDefault();

            }
            
        }
        /// <summary>
        /// 找相關作者的書籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductDetailVm> GetAuthor(string Name)
        {
            using (var db = new AppDbContext())
            {
                var result = db.Products.Where(x => x.Author == Name).Select(x => new ProductDetailVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,               
                    Price = x.Price,
                    ImageURL=x.ImageURL
                }).Take(6).ToList();

                return result;
            }
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="product"></param>
        public void Create(Product product)
        {
            using (var db = new AppDbContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product"></param>
        public void Update(Product product)
        {
            using (var db = new AppDbContext())
            {
                var oldProduct = db.Products.Find(product.Id);
                oldProduct.Title = product.Title;
                oldProduct.Author = product.Author;
                oldProduct.Publisher = product.Publisher;
                oldProduct.Price = product.Price;
                oldProduct.Description = product.Description;
                oldProduct.CategoryId = product.CategoryId;
                oldProduct.PromotionId = product.PromotionId;
                oldProduct.PublishDate = product.PublishDate;
                oldProduct.ImageURL = product.ImageURL;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var db = new AppDbContext())
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }




		public List<Product> GetBooks()
		{
			using (var db = new AppDbContext())
			{
				return db.Products.Take(6).ToList();
			}
		}
		/// <summary>
		/// 首頁根據商品 Id 取得商品資訊（包含照片、書名、價錢）
		/// </summary>
		/// <param name="Id">商品的 Id</param>
		/// <returns>商品資訊的列表</returns>
		public Product GetProductById(int Id)
		{
			using (var db = new AppDbContext())
			{
				// 查詢符合條件的商品
				var product = db.Products
					.Where(p => p.Id == Id)
					.Select(p => new Product
					{
						Id = p.Id,
						Title = p.Title,
						Price = p.Price,
						ImageURL = p.ImageURL
					})
					.FirstOrDefault();

				return product;
			}
		}

	}
}