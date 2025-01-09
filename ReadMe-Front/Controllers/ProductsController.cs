using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReadMe_Front.Controllers
{

    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        public ProductsController()
        {
            _productService = new ProductService();
        }
        // GET: Products
        /// <summary>
        /// 單一商品資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        public ActionResult ProductDetail(int id)
        {
            try
            {
                var productDetail = _productService.GetProductById(id);

                if (productDetail == null)
                {
                    return HttpNotFound("找不到商品");
                }

                return View(productDetail);
            }
            catch (KeyNotFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "發生未知錯誤：" + ex.Message;
                return View("Error");
            }
        }
        public ActionResult ProductAuthor(string Author)
        {

            var AuthorBook = _productService.GetAuthorBook(Author);
            if (AuthorBook == null)
            {
                return HttpNotFound("找不到相關作者");
            }
            return View(AuthorBook);
        }
        /// <summary>
        /// 收藏清單
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Favorite()
        {

            return View();
        }
        /// <summary>
        /// 無收藏清單的page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult NoFavorite()
        {
            return View();
        }

       





    }
}