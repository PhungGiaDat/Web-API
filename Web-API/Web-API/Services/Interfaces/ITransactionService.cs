using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse> EarnPointsAsync(EarnPointsRequest request);
        Task<ApiResponse> RedeemPointsAsync(RedeemPointsRequest request);
        Task<ApiResponse> GetTransactionHistoryAsync(string cardNumber, int page = 1, int pageSize = 10);
        Task<ApiResponse> GetTransactionByIdAsync(string transactionId);
    }
}