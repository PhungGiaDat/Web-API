using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface IShopService
    {
        Task<List<ShopResponse>> GetAllShopsAsync();
        Task<SimpleApiResponse> CreateShopAsync(CreateShopRequest request);
        Task<SimpleApiResponse> UpdateShopAsync(string shopId, UpdateShopRequest request);
        Task<SimpleApiResponse> DeleteShopAsync(string shopId);
        Task<bool> ValidateShopApiKeyAsync(string shopId, string apiKey);
    }
}