namespace Web_API.Security.Interfaces
{
    public interface IApiKeyService
    {
        Task<bool> ValidateApiKeyAsync(string apiKey);
        Task<string?> GetShopIdFromApiKeyAsync(string apiKey);
        Task<string> GenerateApiKeyAsync(string shopId);
        Task<bool> RevokeApiKeyAsync(string shopId);
    }
}