using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Services.Interfaces;

namespace Web_API.Controllers
{
    [Route("api/loyalty")]
    [ApiController]
    public class LoyaltyTransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public LoyaltyTransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Earn points after purchase at shop
        /// Ghi nhận tích điểm sau khi mua hàng tại cửa hàng
        /// </summary>
        [HttpPost("earn")]
        //[Authorize(Policy = "ShopOnly")]
        public async Task<IActionResult> EarnPoints([FromBody] EarnPointsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _transactionService.EarnPointsAsync(request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Get transaction history for a loyalty card
        /// Xem lịch sử giao dịch điểm (tích điểm / đổi quà) theo khoảng thời gian
        /// </summary>
        [HttpGet("history")]
        [Authorize(Policy = "CustomerOwnership")]
        public async Task<IActionResult> GetTransactionHistory([FromQuery] string cardNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return BadRequest(new { message = "Card number is required" });
            }

            var result = await _transactionService.GetTransactionHistoryAsync(cardNumber, page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Get specific transaction by ID
        /// Lấy thông tin giao dịch cụ thể theo ID
        /// </summary>
        [HttpGet("transaction/{transactionId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetTransactionById(string transactionId)
        {
            var result = await _transactionService.GetTransactionByIdAsync(transactionId);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
