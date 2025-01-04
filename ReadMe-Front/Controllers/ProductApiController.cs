using ReadMe_Front.Models.Services;
using System.Linq;
using System.Web.Http;

namespace ReadMe_Front.Controllers.Api
{
    public class ProductApiController : ApiController
    {
        private readonly ProductService _productService;

        public ProductApiController()
        {
            _productService = new ProductService();
        }

        // GET api/productapi/favorites
        [HttpGet]
        [Route("api/productapi/favorites")]
        public IHttpActionResult Favorites( int userid)
        {
            var favorites = _productService.GetFavoriteProducts(userid);

            if (favorites == null || !favorites.Any())
            {
                return Ok(new { hasItems = false, message = "目前沒有收藏商品" });
            }

            return Ok(new { hasItems = true, items = favorites });
        }
    }
}
