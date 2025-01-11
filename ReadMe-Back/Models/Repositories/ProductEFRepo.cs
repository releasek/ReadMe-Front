using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.DTOs;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Repositories
{
    public class ProductEFRepo
    {
        private readonly AppDbContext _db;

        public ProductEFRepo(AppDbContext db)
        {
            _db = db;
        }

        public IQueryable<Product> GetProducts()
        {
            var products = _db.Products.Include(c => c.Category).Include(p => p.Category.ParentCategory);
            return products;
        }
        public IQueryable<CategoryDto> AllCategories()
        {
            var categories = _db.Categories.Select(c => new CategoryDto
            {
                Id = c.Id, // 假設 Category 有 Id 屬性
                CategoryName = c.CategoryName
            });
            return categories;
        }

        public void UpdateProduct(Product product)
        {
            _db.Products.Update(product); // 標記產品為已修改
            _db.SaveChanges();            // 保存所有更改
        }

    }
}
