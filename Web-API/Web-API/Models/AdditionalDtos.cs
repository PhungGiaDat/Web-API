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
        
        [Range(1, int.MaxValue, ErrorMessage = "Points must be greater than 0")]
        public int Points { get; set; }
        
        public string? Note { get; set; }
    }

    public class RedeemPointsRequest
    {
        [Required]
        public string CardNumber { get; set; } = null!;
        
        [Required]
        public string RewardId { get; set; } = null!;
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
}