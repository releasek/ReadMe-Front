using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadMe_Back.Models.EFModels;
using ReadMe_Back.Models.Repositories;
using ReadMe_Back.Models.Services;

namespace ReadMe_Back.Controllers.ApisController
{
    [Route("api/promotion")]
    [ApiController]
    public class PromotionApiController : ControllerBase
    {
        private readonly PromotionService _promotionService;
        public PromotionApiController(PromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllPromotion()
        {
            var promotions = await _promotionService.GetPromotions();
            return Ok(promotions);
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreatePromotion([FromBody] Promotion promotion)
        {
            if (promotion == null) return BadRequest("請輸入優惠");
            try
            {
                await _promotionService.AddPromotions(promotion);
                return Ok(new { Message = "新增成功", Promotion = promotion.PromotionName });
            }
            catch (ArgumentException)
            {
                return BadRequest("優惠名稱不可為空");
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePromotiom(int id)
        {
            try
            {
                await _promotionService.DeletePromotions(id);
                return Ok(new {message="刪除成功"});
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("找不到促銷活動");
            }

        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] Promotion promotion)
        {
            if (promotion == null) return BadRequest("請輸入優惠");
            try
            {
                await _promotionService.UpdatePrmotions(promotion);
                return Ok(new {message="更新成功"});
            }
            catch (KeyNotFoundException)
            {
                return BadRequest("找不到優惠");
            }
        }
    }

}
