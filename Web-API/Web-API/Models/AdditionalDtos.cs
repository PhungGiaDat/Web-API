using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


// DTOS defining the structure of requests for various operations in the loyalty program


// Các ly yêu cầu để thực hiện các thao tác trong chương trình khách hàng thân thiết
// Định nghĩa lại cấu trúc API chấp nhận
namespace Web_API.Models
{
    
    // Validate lại request nhận điểm
    public class EarnPointsRequest
    {
        [Required]
        public string CardNumber { get; set; } = null!;
        
        [Required]
        public string ShopId { get; set; } = null!;
        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount spent must be greater than 0")]
        public decimal AmountSpent { get; set; }
        
        public string? Note { get; set; }
    }

    // Response for earn points API
    public class EarnPointsResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("earnedPoints")]
        public int EarnedPoints { get; set; }
        
        [JsonPropertyName("totalPoints")]
        public int TotalPoints { get; set; }
        
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = null!;
    }

    public class RedeemPointsRequest
    {
        [Required]
        public string CardNumber { get; set; } = null!;
        
        [Required]
        public string RewardId { get; set; } = null!;
        
        public string? Note { get; set; }
    }

    // Response for redeem points API
    public class RedeemPointsResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("redeemedPoints")]
        public int RedeemedPoints { get; set; }
        
        [JsonPropertyName("remainingPoints")]
        public int RemainingPoints { get; set; }
        
        [JsonPropertyName("rewardName")]
        public string RewardName { get; set; } = null!;
        
        [JsonPropertyName("redeemId")]
        public string RedeemId { get; set; } = null!;
    }

    // Individual transaction for history
    public class TransactionHistoryItem
    {
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = null!;
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
        
        [JsonPropertyName("points")]
        public int Points { get; set; }
        
        [JsonPropertyName("transactionTime")]
        public string TransactionTime { get; set; } = null!;
        
        [JsonPropertyName("note")]
        public string Note { get; set; } = null!;
        
        [JsonPropertyName("balanceAfter")]
        public int BalanceAfter { get; set; }
    }

    // Reward response for GET /api/rewards
    public class RewardResponse
    {
        [JsonPropertyName("rewardId")]
        public string RewardId { get; set; } = null!;
        
        [JsonPropertyName("rewardName")]
        public string RewardName { get; set; } = null!;
        
        [JsonPropertyName("requiredPoints")]
        public int RequiredPoints { get; set; }
        
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    // Top customer for statistics
    public class TopCustomer
    {
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; } = null!;
        
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = null!;
        
        [JsonPropertyName("pointsRedeemed")]
        public int PointsRedeemed { get; set; }
    }

    // System statistics response
    public class SystemStatisticsResponse
    {
        [JsonPropertyName("totalCards")]
        public int TotalCards { get; set; }
        
        [JsonPropertyName("totalPointsRedeemed")]
        public int TotalPointsRedeemed { get; set; }
        
        [JsonPropertyName("topCustomers")]
        public List<TopCustomer> TopCustomers { get; set; } = new List<TopCustomer>();
    }

    public class CreateRewardRequest
    {
        [Required]
        public string RewardId { get; set; } = null!;
        
        [Required]
        public string RewardName { get; set; } = null!;
        
        public string? Description { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Required points must be greater than 0")]
        public int RequiredPoints { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    public class UpdateRewardRequest
    {
        [Required]
        public string RewardName { get; set; } = null!;
        
        public string? Description { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Required points must be greater than 0")]
        public int RequiredPoints { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class CreateShopRequest
    {
        [Required]
        public string ShopId { get; set; } = null!;
        
        [Required]
        public string ShopName { get; set; } = null!;
        
        public string? Address { get; set; }
    }

    public class UpdateShopRequest
    {
        [Required]
        public string ShopName { get; set; } = null!;
        
        public string? Address { get; set; }
    }

    // DTO for loyalty card details response
    public class LoyaltyCardDetailsResponse
    {
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; } = null!;
        
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = null!;
        
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = null!;
        
        [JsonPropertyName("citizenId")]
        public string CitizenId { get; set; } = null!;
        
        [JsonPropertyName("expiryDate")]
        public string ExpiryDate { get; set; } = null!;
        
        [JsonPropertyName("availablePoints")]
        public int AvailablePoints { get; set; }
    }

    // DTO for updating customer information
    public class UpdateCustomerInfoRequest
    {
        [Required]
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = null!;
        
        [Required]
        [Phone]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = null!;
        
        [Required]
        [StringLength(12, MinimumLength = 12)]
        [JsonPropertyName("citizenId")]
        public string CitizenId { get; set; } = null!;
        
        [Required]
        [JsonPropertyName("expiryDate")]
        public string ExpiryDate { get; set; } = null!;
    }

    // Simple response DTO for operations that only need status and message
    public class SimpleApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;
    }

    // Individual shop response DTO
    public class ShopResponse
    {
        [JsonPropertyName("shopId")]
        public string ShopId { get; set; } = null!;
        
        [JsonPropertyName("shopName")]
        public string ShopName { get; set; } = null!;
        
        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}