using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.Security;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Controllers
{
    //[Authorize]
    [FunctionAuthorize("商品管理")]
    public class ProductsController : Controller
    {
        private readonly ProductEFRepo _repo;

        public ProductsController(ProductEFRepo repo)
        {
            _repo = repo;
        }

        // GET: AllProducts
        public IActionResult Index()
        {
            var allCategories = _repo.AllCategories().Distinct().ToList();
            var products = _repo.GetProducts().Select(p => new ProductVm
            {
                Id = p.Id,
                Title = p.Title,
                Author = p.Author,
                Publisher = p.Publisher,
                Description = p.Description,
                Price = p.Price,
                PublishDate = p.PublishDate,
                CategoryName = p.Category.CategoryName,
                ParentCategoryName = p.Category.ParentCategory.ParentCategoriesName,
                ImageURL = p.ImageUrl,
            }).ToList();

            var vm = new ProductIndexVm
            {
                Products = products,
                AllCategories = allCategories
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult GetProduct(int id)
        {
            var product = _repo.GetProducts().FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Json(new
            {
                id = product.Id,
                title = product.Title,
                author = product.Author,
                publisher = product.Publisher,
                price = product.Price,
                publishdate = product.PublishDate?.ToString("yyyy-MM-dd"),
                imageurl = product.ImageUrl,
                categoryid = product.CategoryId,
                categoryname = product.Category.CategoryName,
                description = product.Description
            });
        }

        [HttpPost]
        public IActionResult Update(ProductVm model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid product data." });
            }

            var product = _repo.GetProducts().FirstOrDefault(p => p.Id == model.Id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            // 更新產品的屬性
            product.Title = model.Title;
            product.Author = model.Author;
            product.Publisher = model.Publisher;
            product.Price = model.Price;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.PublishDate = model.PublishDate;
            product.ImageUrl = model.ImageURL;

            try
            {
                // 保存更改到資料庫
                _repo.UpdateProduct(product); // 假設在 Repo 中實現 Update 方法

                // 設置 TempData，傳遞成功消息
                TempData["SuccessMessage"] = "修改成功";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "商品更新失敗", error = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult Create(ProductVm model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid product data." });
            }

            var product = new Product();

            // 更新產品的屬性
            product.Title = model.Title;
            product.Author = model.Author;
            product.Publisher = model.Publisher;
            product.Price = model.Price;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.PublishDate = model.PublishDate;
            product.ImageUrl = model.ImageURL;

            try
            {
                // 保存更改到資料庫
                _repo.CreateProduct(product);

                // 設置 TempData，傳遞成功消息
                TempData["SuccessMessage"] = "新增成功";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "商品新增失敗", error = ex.Message });
            }

        }


        public IActionResult Delete(int id)
        {
            bool result = false;
            var product = _repo.GetProducts().FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                result = true;
                _repo.DeleteProduct(product);
            }
            // 設置 TempData，傳遞成功消息
            TempData["SuccessMessage"] = "刪除成功";
            return RedirectToAction("Index");
        }

    }
}
