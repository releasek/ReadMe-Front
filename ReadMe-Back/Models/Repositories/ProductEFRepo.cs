using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Repositories
{
    public class ProductEFRepo
    {
        private readonly AppDbContext _db;
        public ProductEFRepo(AppDbContext db)
        {
            this._db = db;
        }

        public void GetProducts()
        {
        }
    }
}
