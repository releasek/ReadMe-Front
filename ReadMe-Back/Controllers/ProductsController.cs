using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.ViewModels;

namespace ReadMe_Back.Controllers
{
    [Authorize]
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

        public JsonResult Delete(int id)
        {
            bool result = false;
            var product = _repo.GetProducts().FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                result = true;
                _repo.DeleteProduct(product);
            }
            return Json(result);
        }

        //// GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Promotion)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
        //    ViewData["PromotionId"] = new SelectList(_context.Promotions, "Id", "PromotionName");
        //    return View();
        //}

        //// POST: Products/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Author,Publisher,Price,Description,CategoryId,PromotionId,PublishDate,ImageUrl")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
        //    ViewData["PromotionId"] = new SelectList(_context.Promotions, "Id", "PromotionName", product.PromotionId);
        //    return View(product);
        //}



        //// POST: Products/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Publisher,Price,Description,CategoryId,PromotionId,PublishDate,ImageUrl")] Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
        //    ViewData["PromotionId"] = new SelectList(_context.Promotions, "Id", "PromotionName", product.PromotionId);
        //    return View(product);
        //}

        //// GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Promotion)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Products.Remove(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.Id == id);
        //}
    }
}
