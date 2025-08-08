using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Services.Interfaces;

namespace Web_API.Controllers
{
    [Route("api/shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        /// <summary>
        /// Get all shops
        /// L?y danh s�ch t?t c? c?a h�ng
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllShops()
        {
            var shops = await _shopService.GetAllShopsAsync();
            return Ok(shops);
        }

        /// <summary>
        /// Create a new shop (Admin only)
        /// T?o c?a h�ng m?i (Ch? admin)
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shopService.CreateShopAsync(request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        /// <summary>
        /// Update shop information
        /// C?p nh?t th�ng tin c?a h�ng
        /// </summary>
        [HttpPut("{shopId}")]
        public async Task<IActionResult> UpdateShop(string shopId, [FromBody] UpdateShopRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shopService.UpdateShopAsync(shopId, request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        /// <summary>
        /// Delete a shop
        /// X�a c?a h�ng
        /// </summary>
        [HttpDelete("{shopId}")]
        public async Task<IActionResult> DeleteShop(string shopId)
        {
            var result = await _shopService.DeleteShopAsync(shopId);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}