using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API.Models;
using Web_API.Services.Interfaces;

namespace Web_API.Controllers
{
    [Route("api/loyalty")]
    [ApiController]
    public class LoyaltyCardsController : ControllerBase
    {
        private readonly ILoyaltyCardService _loyaltyCardService;

        public LoyaltyCardsController(ILoyaltyCardService loyaltyCardService)
        {
            _loyaltyCardService = loyaltyCardService;
        }

        // POST api/loyalty/register
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> RegisterLoyaltyCard([FromBody] RegisterLoyaltyCardRequest request)
        {
            var result = await _loyaltyCardService.RegisterLoyaltyCardAsync(request);

            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET api/loyalty/{cardNumber}
        [HttpGet("{cardNumber}")]
        public async Task<ActionResult> GetByCardNumber(string cardNumber)
        {
            var result = await _loyaltyCardService.GetByCardNumberAsync(cardNumber);
            
            if (result.Status == "SUCCESS")
            {
                // Return the specific structure for GET endpoint
                var response = new LoyaltyCardDetailsResponse
                {
                    CardNumber = result.CardNumber!,
                    CustomerName = result.CustomerName!,
                    Phone = result.Phone!,
                    CitizenId = result.CitizenId!,
                    ExpiryDate = result.ExpiryDate!,
                    AvailablePoints = result.AvailablePoints!.Value
                };
                return Ok(response);
            }
            else
            {
                return NotFound(result);
            }
        }

        // PUT api/loyalty/{cardNumber}/update-info
        [HttpPut("{cardNumber}/update-info")]
        public async Task<ActionResult<SimpleApiResponse>> UpdateCardInfo(string cardNumber, [FromBody] UpdateCustomerInfoRequest request)
        {
            var result = await _loyaltyCardService.UpdateCardInfoAsync(cardNumber, request);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // DELETE api/loyalty/{cardNumber} - Admin Only
        [HttpDelete("{cardNumber}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<SimpleApiResponse>> DeleteCard(string cardNumber)
        {
            var result = await _loyaltyCardService.DeleteCardAsync(cardNumber);
            
            if (result.Status == "SUCCESS")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
