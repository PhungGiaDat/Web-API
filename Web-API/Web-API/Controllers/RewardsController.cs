using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Services.Interfaces;

namespace Web_API.Controllers
{
    [Route("api/rewards")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IRewardService _rewardService;

        public RewardsController(ITransactionService transactionService, IRewardService rewardService)
        {
            _transactionService = transactionService;
            _rewardService = rewardService;
        }

        /// <summary>
        /// Get all available rewards
        /// L?y danh sách ph?n th??ng ?ang có (tên, mô t?, ?i?m yêu c?u)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRewards()
        {
            var rewards = await _rewardService.GetAllRewardsAsync();
            return Ok(rewards);
        }

        /// <summary>
        /// Create a new reward (Admin only)
        /// Thêm ph?n th??ng m?i (Admin dùng)
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateReward([FromBody] CreateRewardRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _rewardService.CreateRewardAsync(request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        /// <summary>
        /// Redeem points for rewards
        /// Khách hàng ??i ph?n th??ng n?u ?? ?i?m (gi?m ?i?m t??ng ?ng, ghi nh?t ký giao d?ch)
        /// </summary>
        [HttpPost("redeem")]
        [Authorize(Policy = "ShopOnly")]
        public async Task<IActionResult> RedeemPoints([FromBody] RedeemPointsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _transactionService.RedeemPointsAsync(request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}