using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}