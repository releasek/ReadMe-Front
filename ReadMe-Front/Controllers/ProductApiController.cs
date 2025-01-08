using ReadMe_Front.Models.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace ReadMe_Front.Controllers
{
    public class ProductApiController : ApiController
    {
        private readonly ProductService _productService;
        public ProductApiController()
        {
            _productService = new ProductService();
        }
        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cartapi/getProductDetail")]
        public IHttpActionResult GetProduct(int id)
        {
            try
            {
                var productDetail = _productService.GetProductById(id);

                if (productDetail == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "找不到商品" }); //404找不到商品
                }

                return Ok(productDetail);
            }
            catch (KeyNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, new { Message = ex.Message }); //404找不到商品
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "發生未知錯誤：" + ex.Message }); //500內部錯誤
            }
        }
        /// <summary>
        /// 取得作者的書籍
        /// </summary>
        /// <param name="Author"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/cartapi/getAuthor")]
        public IHttpActionResult GetAuthorBook(string author)
        {
            try
            {
                var AuthorBook = _productService.GetAuthorBook(author);
                if (AuthorBook == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = "找不到相關作者" });
                }
                return Ok(AuthorBook);
            }
            catch (KeyNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { Message = "發生未知錯誤：" + ex.Message });
            }
        }
    }
}
