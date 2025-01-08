using ReadMe_Front.Models.Services;
using System.Linq;
using System.Net;
using System.Web.Http;


namespace ReadMe_Front.Controllers
{
    public class FavoriteApiController : ApiController
    {
        private readonly ProductService _productService;
        public FavoriteApiController()
        {
            _productService = new ProductService();
        }
        /// <summary>
        /// 取得收藏的商品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFavorites()
        {
            // 取得目前使用者的帳號
            string account = User.Identity.Name;

            // 從服務層獲取收藏的商品
            var favoriteProducts = _productService.GetFavoriteProducts(account);

            // 檢查是否有收藏的商品
            if (favoriteProducts == null || !favoriteProducts.Any())
            {
                // 返回 404 並帶上錯誤訊息
                return Content(HttpStatusCode.NotFound, new { Message = "目前沒有收藏的商品" });
            }

            // 返回收藏的商品列表
            return Ok(favoriteProducts);
        }
        [HttpDelete]
        public IHttpActionResult DeleteFavorite(int id)
        {
            // 從服務層刪除收藏
            _productService.DeleteFavorite(id);

            // 返回成功訊息
            return Ok(new { Message = "刪除收藏成功" });
        }


    }
}
