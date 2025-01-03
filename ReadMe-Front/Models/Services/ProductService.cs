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
        
        public List<ProductDetailVm> GetAuthorBook(string Author)
        {
            var authorBook = _productRepo.GetAuthor(Author);
            if (authorBook == null || !authorBook.Any())
            {
                throw new KeyNotFoundException($"未找到相關作者{Author}的書籍");
            }
            return authorBook;
        }
    }
}