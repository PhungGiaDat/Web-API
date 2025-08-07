using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface IShopService
    {
        Task<ApiResponse> GetAllShopsAsync();
        Task<ApiResponse> GetShopByIdAsync(string shopId);
        Task<ApiResponse> CreateShopAsync(CreateShopRequest request);
        Task<ApiResponse> UpdateShopAsync(string shopId, UpdateShopRequest request);
        Task<ApiResponse> DeleteShopAsync(string shopId);
        Task<bool> ValidateShopApiKeyAsync(string shopId, string apiKey);
    }
}