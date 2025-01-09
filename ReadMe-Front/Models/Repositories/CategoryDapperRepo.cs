using Dapper;
using ReadMe_Front.Models.DTOs;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class CategoryDapperRepo
    {
        private string _connString;

        public CategoryDapperRepo()
        {
            _connString = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext1"].ToString();
        }

        public List<CategoryDto> GetProducts()
        {
            string sql = @"Select p.Id, p.Title, p.Author, p.Price, p.ImageURL, c.CategoryName, pc.ParentCategoriesName From Products p
Inner Join Categories c on p.CategoryId = c.Id
Inner Join ParentCategories pc on c.ParentCategoryId = pc.Id
Order by pc.ParentCategoriesName, p.Id";

            using (var conn = new SqlConnection(_connString))
            {
                return conn.Query<CategoryDto>(sql).ToList();
            }
        }

        public List<CategoryDto> GetCategories()
        {
            string sql = @"Select c.CategoryName, pc.ParentCategoriesName From Categories c
Inner Join ParentCategories pc on c.ParentCategoryId = pc.Id
Order by pc.ParentCategoriesName";

            using (var conn = new SqlConnection(_connString))
            {
                return conn.Query<CategoryDto>(sql).ToList();
            }
        }
    }
}