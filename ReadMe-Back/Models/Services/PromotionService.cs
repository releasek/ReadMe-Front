using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;

namespace ReadMe_Back.Models.Services
{
    public class PromotionService
    {
        private readonly PromotionEFRepo _promotionRepo;
        public PromotionService(PromotionEFRepo productEFRepo)
        {
            _promotionRepo = productEFRepo;
        }
        public async Task<List<Promotion>> GetPromotions()
        {
            return await _promotionRepo.GetPromotions();
        }
        public async Task AddPromotions(Promotion promotion)
        {
            if (string.IsNullOrWhiteSpace("優惠名稱不可為空"))
            {
                throw new ArgumentException("優惠名稱不可為空");
            }
            await _promotionRepo.Create(promotion);
        }
        public async Task DeletePromotions(int id)
        {
            await _promotionRepo.Delete(id);
        }
        public async Task UpdatePrmotions(Promotion promotion)
        {
            if (string.IsNullOrWhiteSpace(promotion.PromotionName))
            {
                throw new KeyNotFoundException("找不到優惠");
            }
            await _promotionRepo.Update(promotion);
        }
    }
}
