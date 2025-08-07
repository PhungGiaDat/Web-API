// Định nghĩa ra một đối tượng ApiResponse để trả về kết quả từ API
namespace Web_API.Models
{
    public class ApiResponse
    {
        public string Status { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? CardNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? CitizenId { get; set; }
        public string? ExpiryDate { get; set; }
        public int? AvailablePoints { get; set; }
    }
}