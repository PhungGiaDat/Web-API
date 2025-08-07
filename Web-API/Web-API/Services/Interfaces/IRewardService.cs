using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface IRewardService
    {
        Task<ApiResponse> GetAllRewardsAsync();
        Task<ApiResponse> GetActiveRewardsAsync();
        Task<ApiResponse> GetRewardByIdAsync(string rewardId);
        Task<ApiResponse> CreateRewardAsync(CreateRewardRequest request);
        Task<ApiResponse> UpdateRewardAsync(string rewardId, UpdateRewardRequest request);
        Task<ApiResponse> DeleteRewardAsync(string rewardId);
        Task<ApiResponse> GetAffordableRewardsAsync(string cardNumber);
    }
}