using Web_API.Services.Interfaces;
using Web_API.Repositories.Interfaces;
using Web_API.Data;
using Web_API.Models;

namespace Web_API.Services
{
    public class RewardService : IRewardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RewardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RewardResponse>> GetAllRewardsAsync()
        {
            try
            {
                var rewards = await _unitOfWork.Rewards.GetAllActiveAsync();
                
                return rewards.Select(r => new RewardResponse
                {
                    RewardId = r.RewardId,
                    RewardName = r.RewardName,
                    RequiredPoints = r.RequiredPoints,
                    Description = r.Description
                }).ToList();
            }
            catch (Exception)
            {
                return new List<RewardResponse>();
            }
        }

        public async Task<ApiResponse> GetActiveRewardsAsync()
        {
            try
            {
                var rewards = await _unitOfWork.Rewards.GetAllActiveAsync();
                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "L?y danh sách ph?n th??ng ?ang ho?t ??ng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi l?y danh sách ph?n th??ng"
                };
            }
        }

        public async Task<ApiResponse> GetRewardByIdAsync(string rewardId)
        {
            if (string.IsNullOrEmpty(rewardId))
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Mã ph?n th??ng là b?t bu?c."
                };
            }

            try
            {
                var reward = await _unitOfWork.Rewards.GetByIdAsync(rewardId);
                if (reward == null)
                {
                    return new ApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm th?y ph?n th??ng."
                    };
                }

                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "L?y thông tin ph?n th??ng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi l?y thông tin ph?n th??ng"
                };
            }
        }

        public async Task<SimpleApiResponse> CreateRewardAsync(CreateRewardRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RewardId) || string.IsNullOrEmpty(request.RewardName))
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "D? li?u ph?n th??ng không h?p l?."
                };
            }

            try
            {
                // Check if reward already exists
                var existingReward = await _unitOfWork.Rewards.GetByIdAsync(request.RewardId);
                if (existingReward != null)
                {
                    return new SimpleApiResponse
                    {
                        Status = "Failed",
                        Message = "Ph?n th??ng v?i mã này ?ã t?n t?i."
                    };
                }

                var reward = new Reward
                {
                    RewardId = request.RewardId,
                    RewardName = request.RewardName,
                    Description = request.Description,
                    RequiredPoints = request.RequiredPoints,
                    IsActive = request.IsActive
                };

                await _unitOfWork.Rewards.CreateAsync(reward);
                await _unitOfWork.SaveChangesAsync();

                return new SimpleApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Thêm ph?n th??ng thành công"
                };
            }
            catch (Exception ex)
            {
                return new SimpleApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi t?o ph?n th??ng"
                };
            }
        }

        public async Task<ApiResponse> UpdateRewardAsync(string rewardId, UpdateRewardRequest request)
        {
            if (string.IsNullOrEmpty(rewardId) || request == null || string.IsNullOrEmpty(request.RewardName))
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "D? li?u ph?n th??ng không h?p l?."
                };
            }

            try
            {
                var reward = await _unitOfWork.Rewards.GetByIdAsync(rewardId);
                if (reward == null)
                {
                    return new ApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm th?y ph?n th??ng."
                    };
                }

                reward.RewardName = request.RewardName;
                reward.Description = request.Description;
                reward.RequiredPoints = request.RequiredPoints;
                reward.IsActive = request.IsActive;

                await _unitOfWork.Rewards.UpdateAsync(reward);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "C?p nh?t ph?n th??ng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi c?p nh?t ph?n th??ng"
                };
            }
        }

        public async Task<ApiResponse> DeleteRewardAsync(string rewardId)
        {
            if (string.IsNullOrEmpty(rewardId))
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Mã ph?n th??ng là b?t bu?c."
                };
            }

            try
            {
                var reward = await _unitOfWork.Rewards.GetByIdAsync(rewardId);
                if (reward == null)
                {
                    return new ApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm th?y ph?n th??ng."
                    };
                }

                await _unitOfWork.Rewards.DeleteAsync(rewardId);
                await _unitOfWork.SaveChangesAsync();

                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "Xóa ph?n th??ng thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi xóa ph?n th??ng"
                };
            }
        }

        public async Task<ApiResponse> GetAffordableRewardsAsync(string cardNumber)
        {
            try
            {
                var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(cardNumber);
                if (card == null)
                {
                    return new ApiResponse
                    {
                        Status = "Failed",
                        Message = "Không tìm th?y th? khách hàng thân thi?t."
                    };
                }

                var rewards = await _unitOfWork.Rewards.GetAffordableRewardsAsync(card.AvailablePoints);
                return new ApiResponse
                {
                    Status = "SUCCESS",
                    Message = "L?y danh sách ph?n th??ng có th? ??i thành công"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Có l?i x?y ra khi l?y danh sách ph?n th??ng"
                };
            }
        }
    }
}