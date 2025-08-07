using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface ILoyaltyCardService
    {
        Task<ApiResponse> RegisterLoyaltyCardAsync(RegisterLoyaltyCardRequest request);

        Task<ApiResponse> GetByCardNumberAsync(string cardNumber);
        Task<SimpleApiResponse> UpdateCardInfoAsync(string cardNumber, UpdateCustomerInfoRequest request);
        Task<SimpleApiResponse> DeleteCardAsync(string cardNumber);
      
    }
}