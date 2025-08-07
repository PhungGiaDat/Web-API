using Web_API.Data;

namespace Web_API.Repositories.Interfaces
{
    public interface IRewardRepository
    {
        Task<Reward?> GetByIdAsync(string rewardId);
        Task<IEnumerable<Reward>> GetAllActiveAsync();
        Task<IEnumerable<Reward>> GetAllAsync();
        Task<Reward> CreateAsync(Reward reward);
        Task<Reward> UpdateAsync(Reward reward);
        Task DeleteAsync(string rewardId);
        Task<bool> ExistsAsync(string rewardId);
        Task<IEnumerable<Reward>> GetAffordableRewardsAsync(int availablePoints);
    }
}