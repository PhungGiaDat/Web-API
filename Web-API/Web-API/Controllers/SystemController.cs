using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Services.Interfaces;

namespace Web_API.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;

        public SystemController(ISystemService systemService)
        {
            _systemService = systemService;
        }

        /// <summary>
        /// Get system statistics
        /// L?y th?ng k� t?ng s? th?, t?ng ?i?m ?� ??i, top kh�ch h�ng t�ch c?c
        /// </summary>
        [HttpGet("statistics")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetSystemStatistics()
        {
            var statistics = await _systemService.GetSystemStatisticsAsync();
            return Ok(statistics);
        }
    }
}