using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{

    // DTO defining the structure of a request to register a new loyalty card
    // DTO định nghĩa cấu trúc của yêu cầu đăng ký thẻ khách hàng thân thiết mới
    public class RegisterLoyaltyCardRequest
    {
        [Required]
        public string CardNumber { get; set; } = null!;
        
        [Required]
        public string CustomerName { get; set; } = null!;
        
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
        
        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string CitizenId { get; set; } = null!;
        
        [Required]
        public string ExpiryDate { get; set; } = null!;
        
        [Range(0, int.MaxValue, ErrorMessage = "Initial points must be greater than or equal to 0")]
        public int InitialPoints { get; set; }
    }
}