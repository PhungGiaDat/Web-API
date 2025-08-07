using Web_API.Data;

namespace Web_API.Repositories.Interfaces
{
    public interface IShopRepository
    {
        Task<Shop?> GetByIdAsync(string shopId);
        Task<Shop?> GetByApiKeyAsync(string apiKey);
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop> CreateAsync(Shop shop);
        Task<Shop> UpdateAsync(Shop shop);
        Task DeleteAsync(string shopId);
        Task<bool> ExistsAsync(string shopId);
        Task<bool> ValidateApiKeyAsync(string shopId, string apiKey);
    }
}