namespace Web_API.Services;

using System.Runtime.InteropServices;
using Web_API.Models;
using Web_API.Repositories.Interfaces;
using Web_API.Services.Interfaces;
using Web_API.Data;


public class LoyaltyTransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private const decimal POINTS_PER_VND = 0.0001m; // 1 point per 10,000 VND

    public LoyaltyTransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EarnPointsResponse> EarnPointsAsync(EarnPointsRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.CardNumber) || 
            string.IsNullOrEmpty(request.ShopId) || request.AmountSpent <= 0)
        {
            return new EarnPointsResponse
            {
                Status = "Failed",
                EarnedPoints = 0,
                TotalPoints = 0,
                TransactionId = ""
            };
        }

        var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(request.CardNumber);
        if (card == null)
        {
            return new EarnPointsResponse
            {
                Status = "Failed",
                EarnedPoints = 0,
                TotalPoints = 0,
                TransactionId = ""
            };
        }

        // Verify shop exists
        var shop = await _unitOfWork.Shops.GetByIdAsync(request.ShopId);
        if (shop == null)
        {
            return new EarnPointsResponse
            {
                Status = "Failed",
                EarnedPoints = 0,
                TotalPoints = 0,
                TransactionId = ""
            };
        }

        // Calculate points earned (1 point per 10,000 VND)
        int pointsEarned = (int)(request.AmountSpent * POINTS_PER_VND);
        if (pointsEarned <= 0)
        {
            pointsEarned = 1; // Minimum 1 point for any purchase
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Generate transaction ID
            var transactionId = "TRX" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + 
                               new Random().Next(1000, 9999).ToString();
            
            // Create transaction record         
            var transaction = new Transaction
            {
                TransactionId = transactionId,
                CardNumber = request.CardNumber,
                TransactionType = "EARN",
                PointsChanged = pointsEarned,
                BalanceAfter = card.AvailablePoints + pointsEarned,
                TransactionTime = DateTime.UtcNow,
                ReferenceId = request.ShopId, // Store ShopId in ReferenceId since Transaction doesn't have ShopId
                Note = request.Note ?? $"Purchase at {shop.ShopName}, Amount: {request.AmountSpent:N0} VND"
            };

            await _unitOfWork.Transactions.CreateAsync(transaction);

            // Update loyalty card points
            await _unitOfWork.LoyaltyCards.UpdatePointsAsync(request.CardNumber, pointsEarned);

            // Save all changes
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            // Get updated card to return current points
            var updatedCard = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(request.CardNumber);

            return new EarnPointsResponse
            {
                Status = "SUCCESS",
                EarnedPoints = pointsEarned,
                TotalPoints = updatedCard?.AvailablePoints ?? card.AvailablePoints + pointsEarned,
                TransactionId = transactionId
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new EarnPointsResponse
            {
                Status = "Failed",
                EarnedPoints = 0,
                TotalPoints = card.AvailablePoints,
                TransactionId = ""
            };
        }
    }

    public async Task<RedeemPointsResponse> RedeemPointsAsync(RedeemPointsRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.CardNumber) || string.IsNullOrEmpty(request.RewardId))
        {
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = 0,
                RewardName = "",
                RedeemId = ""
            };
        }

        var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(request.CardNumber);
        if (card == null)
        {
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = 0,
                RewardName = "",
                RedeemId = ""
            };
        }

        var reward = await _unitOfWork.Rewards.GetByIdAsync(request.RewardId);
        if (reward == null)
        {
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = card.AvailablePoints,
                RewardName = "",
                RedeemId = ""
            };
        }

        if (!reward.IsActive)
        {
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = card.AvailablePoints,
                RewardName = reward.RewardName,
                RedeemId = ""
            };
        }

        if (card.AvailablePoints < reward.RequiredPoints)
        {
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = card.AvailablePoints,
                RewardName = reward.RewardName,
                RedeemId = ""
            };
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Generate redeem ID
            var redeemId = "RDM" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + 
                          new Random().Next(1000, 9999).ToString();

            // Create transaction record
            var transaction = new Transaction
            {
                TransactionId = redeemId,
                CardNumber = request.CardNumber,
                TransactionType = "REDEEM",
                PointsChanged = -reward.RequiredPoints,
                BalanceAfter = card.AvailablePoints - reward.RequiredPoints,
                TransactionTime = DateTime.UtcNow,
                ReferenceId = request.RewardId,
                Note = $"Đổi phần thưởng {reward.RewardName}"
            };

            await _unitOfWork.Transactions.CreateAsync(transaction);

            // Update loyalty card points
            await _unitOfWork.LoyaltyCards.UpdatePointsAsync(request.CardNumber, -reward.RequiredPoints);

            // Save all changes
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return new RedeemPointsResponse
            {
                Status = "SUCCESS",
                RedeemedPoints = reward.RequiredPoints,
                RemainingPoints = card.AvailablePoints - reward.RequiredPoints,
                RewardName = reward.RewardName,
                RedeemId = redeemId
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new RedeemPointsResponse
            {
                Status = "FAIL",
                RedeemedPoints = 0,
                RemainingPoints = card.AvailablePoints,
                RewardName = reward.RewardName,
                RedeemId = ""
            };
        }
    }

    public async Task<List<TransactionHistoryItem>> GetTransactionHistoryAsync(string cardNumber, int page = 1, int pageSize = 10)
    {
        try
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return new List<TransactionHistoryItem>();
            }

            var card = await _unitOfWork.LoyaltyCards.GetByCardNumberAsync(cardNumber);
            if (card == null)
            {
                return new List<TransactionHistoryItem>();
            }

            var transactions = await _unitOfWork.Transactions.GetByCardNumberPaginatedAsync(cardNumber, page, pageSize);
            
            return transactions.Select(t => new TransactionHistoryItem
            {
                TransactionId = t.TransactionId,
                Type = t.TransactionType,
                Points = t.PointsChanged,
                TransactionTime = t.TransactionTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Note = t.Note ?? "",
                BalanceAfter = t.BalanceAfter
            }).ToList();
        }
        catch (Exception)
        {
            return new List<TransactionHistoryItem>();
        }
    }

    public async Task<ApiResponse> GetTransactionByIdAsync(string transactionId)
    {
        if (string.IsNullOrEmpty(transactionId))
        {
            return new ApiResponse
            {
                Status = "Failed",
                Message = "Transaction ID is required."
            };
        }

        try
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
            if (transaction == null)
            {
                return new ApiResponse
                {
                    Status = "Failed",
                    Message = "Transaction not found."
                };
            }

            return new ApiResponse
            {
                Status = "SUCCESS",
                Message = "Transaction retrieved successfully"
                // Note: You might want to add transaction details to the response
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                Status = "Failed",
                Message = $"Error retrieving transaction: {ex.Message}"
            };
        }
    }
}

