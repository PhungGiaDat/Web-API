using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<EarnPointsResponse> EarnPointsAsync(EarnPointsRequest request);
        Task<RedeemPointsResponse> RedeemPointsAsync(RedeemPointsRequest request);
        Task<List<TransactionHistoryItem>> GetTransactionHistoryAsync(string cardNumber, int page = 1, int pageSize = 10);
        Task<ApiResponse> GetTransactionByIdAsync(string transactionId);
    }
}