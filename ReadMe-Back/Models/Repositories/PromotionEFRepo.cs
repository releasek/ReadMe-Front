using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ReadMe_Back.Models.EFModels;

namespace ReadMe_Back.Models.Repositories
{
    public class PromotionEFRepo
    {
        private readonly AppDbContext _db;
        public PromotionEFRepo(AppDbContext db)
        {
            _db= db;
        }
        public async Task<List<Promotion>> GetPromotions()
        {
            return await _db.Promotions.ToListAsync();
        }
        public async Task Create(Promotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion));
            }
            _db.Promotions.Add(promotion);
            await _db.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var promotion = await _db.Promotions.FindAsync(id);
            if (promotion == null)
            {
                throw new KeyNotFoundException("找不到促銷活動");
            }
            _db.Promotions.Remove(promotion);
            await _db.SaveChangesAsync();
        }
        public async Task Update(Promotion promotion)
        {
            if (promotion == null)
            {
                throw new ArgumentNullException(nameof(promotion));
            }
            _db.Entry(promotion).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
