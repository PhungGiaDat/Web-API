using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class RewardRepository : IRewardRepository
    {
        private readonly LoyaltyCardContext _context;

        public RewardRepository(LoyaltyCardContext context)
        {
            _context = context;
        }

        public async Task<Reward?> GetByIdAsync(string rewardId)
        {
            return await _context.Rewards
                .FirstOrDefaultAsync(r => r.RewardId == rewardId);
        }

        public async Task<IEnumerable<Reward>> GetAllActiveAsync()
        {
            return await _context.Rewards
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reward>> GetAllAsync()
        {
            return await _context.Rewards.ToListAsync();
        }

        public async Task<Reward> CreateAsync(Reward reward)
        {
            _context.Rewards.Add(reward);
            return reward;
        }

        public async Task<Reward> UpdateAsync(Reward reward)
        {
            _context.Rewards.Update(reward);
            return reward;
        }

        public async Task DeleteAsync(string rewardId)
        {
            var reward = await _context.Rewards.FirstOrDefaultAsync(r => r.RewardId == rewardId);
            if (reward != null)
            {
                _context.Rewards.Remove(reward);
            }
        }

        public async Task<bool> ExistsAsync(string rewardId)
        {
            return await _context.Rewards.AnyAsync(r => r.RewardId == rewardId);
        }

        public async Task<IEnumerable<Reward>> GetAffordableRewardsAsync(int availablePoints)
        {
            return await _context.Rewards
                .Where(r => r.IsActive && r.RequiredPoints <= availablePoints)
                .ToListAsync();
        }
    }
}