using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

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
        public List<ProductDetailVm> GetAuthor(string Author)
        {
            using (var db = new AppDbContext())
            {
                var result = db.Products.Where(x => x.Author == Author).Select(x => new ProductDetailVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Price = x.Price,
                    ImageURL = x.ImageURL
                }).Take(6).ToList();

                return result;
            }
        }
        /// <summary>
        /// 取得收藏清單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductDetailVm> GetFavorite(string account)
        {
            using (var db = new AppDbContext())
            {
                var userid = db.Users.Where(a => a.Account == account).Select(u => u.Id).FirstOrDefault();
                var result = (from product in db.Products
                              join wishlists in db.Wishlists
                              on product.Id equals wishlists.ProductId
                              where wishlists.UserId == userid
                              select new ProductDetailVm
                              {
                                  Id = product.Id,
                                  Title = product.Title,
                                  Author = product.Author,
                                  Price = product.Price,
                                  ImageURL = product.ImageURL
                              }).ToList();
                return result;
            }
        }
        public void DeleteFavorite(int id)
        {
            using (var db = new AppDbContext())
            {
                var wishlist = db.Wishlists.FirstOrDefault(w => w.ProductId == id );
                db.Wishlists.Remove(wishlist);
                db.SaveChanges();
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
        /// <summary>
        /// 首頁取得促銷書籍
        /// </summary>
        /// <returns></returns>
		public List<Product> GetBooks()
		{
			using (var db = new AppDbContext())
			{
				return db.Products.Take(6).ToList();
			}
		}
        /// <summary>
        /// 首頁取得促銷書籍
        /// </summary>
        /// <returns></returns>
        public List<Product> GetPromoteBooks()
        {
            using (var db = new AppDbContext())
            {
                return db.Products.Where(p => p.PromotionId == 1).Take(6).ToList();
            }
        }
        /// <summary>
        /// 3件85折專區取得書籍
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public List<Product> PromoteBooks(int promotionId)
        {
            using (var db = new AppDbContext())
            {
                // 查詢符合 PromotionId 的產品
                var promoteProducts = db.Products
                                        .Where(p => p.PromotionId == promotionId)
                                        .ToList();
                return promoteProducts;
            }
        }
        /// <summary>
        /// 首頁根據出版日期取得書籍
        /// </summary>
        /// <returns></returns>
        public List <Product> GetBooksByPublishdate()
        {
            using (var db = new AppDbContext())
            {
                return db.Products.OrderBy(x=>x.PublishDate).Take(6).ToList();
            }
        }
        /// <summary>
        /// 最新上架30本書
        /// </summary>
        /// <returns></returns>
        public List<Product> NewBooks()
        {
            using (var db = new AppDbContext())
            {
                return db.Products.OrderBy(x => x.PublishDate).Take(30).ToList();
            }
        }
        /// <summary>
        ///首頁療癒專區取得書籍
        /// </summary>
        /// <returns></returns>
        public List<Product> GetComfortBooks()
        {
            using (var db = new AppDbContext())
            {
                // 篩選 CategoryId 是 11 或 12 的產品，並限制為最多 6 筆
                return db.Products
                         .Where(p => p.CategoryId == 11 || p.CategoryId == 12)
                         .Take(6)
                         .ToList();
            }
        }
        /// <summary>
        ///療癒專區30本書
        /// </summary>
        /// <returns></returns>
        public List<Product> ComfortBooks()
        {
            using (var db = new AppDbContext())
            {
                return db.Products.Where(x => x.CategoryId == 11 || x.CategoryId == 12).Take(30).ToList();
            }
        }
        /// <summary>
        /// 首頁取得科技與生活書籍
        /// </summary>
        /// <returns></returns>
        public List<Product> GetTechBooks()
        {
            using (var db = new AppDbContext())
            {
                // 篩選 ParentCategoryId 是 3 的產品，並限制為最多 6 筆
                return db.Products
                         .Where(p => p.Category.ParentCategoryId == 3)
                         .Take(6)
                         .ToList();
            }
        }

        public List<Product> TechBooks()
        {
            using (var db = new AppDbContext())
            {
                // 篩選 ParentCategoryId 是 3 的產品
                return db.Products
                         .Where(p => p.Category.ParentCategoryId == 3)
                         .Take(30)
                         .ToList();
            }
        }

    }
}