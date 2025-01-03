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
        public List<ProductDetailVm> GetProductById(int id)
        {
           var productItem = _productRepo.GetById(id);
            if (productItem == null || !productItem.Any())
            {
                throw new Exception("找不到商品");
            }
            return productItem;
        }
    }
}