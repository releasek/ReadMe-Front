using ReadMe_Front.Models.EFModels;
using ReadMe_Front.Models.Repositories;
using ReadMe_Front.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadMe_Front.Models.Services
{
    public class ProductService
    {
        private readonly ProductEFRepo _productRepo;
        public ProductService()
        {
            _productRepo = new ProductEFRepo();
        }
        /// <summary>
        /// 找每項商品的詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ProductDetailVm GetProductById(int id)
        {
            var productItem = _productRepo.GetById(id);

            if (productItem == null)
            {
                throw new KeyNotFoundException("找不到指定的商品。");
            }

            return productItem;
        }  
        /// <summary>
        /// 取得收藏資料
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<ProductDetailVm> GetFavoriteProducts(string account)
        {
            return _productRepo.GetFavorite(account);
        }
        public List<ProductDetailVm> GetAuthorBook(string Author)
        {
            var authorBook = _productRepo.GetAuthor(Author);
            if (authorBook == null || !authorBook.Any())
            {
                throw new KeyNotFoundException($"未找到相關作者{Author}的書籍");
            }
            return authorBook;
        }
        /// <summary>
        /// 刪除收藏
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFavorite(int id)
        {
            _productRepo.DeleteFavorite(id);
        }
    }

    public class GetProductService
    {
		private readonly ProductEFRepo _productRepo;

		/// <summary>
		/// 回傳五筆熱銷商品
		/// </summary>
		/// <returns></returns>
		public List<Product> GetPromoteProducts() { 
			return Enumerable.Range(1, 5)
				.Select(i => new Product { Id = i, Title = $"商品{i}", Price = i * 100 })
				.ToList();
		}

		public GetProductService()
		{
			_productRepo = new ProductEFRepo();
		}
		// 根據商品 Id 取得商品資訊（包含照片、書名、價錢）
		//public Product GetProductById(int id)
		//{
		//	// 呼叫資料存取層的對應方法
		//	var product = _productRepo.GetProductById(id);

		//	if (product == null)
		//	{
		//		throw new Exception($"找不到 Id 為 {id} 的商品");
		//	}

		//	return product;
		//}

	}

}