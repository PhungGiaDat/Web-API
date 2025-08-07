using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly LoyaltyCardContext _context;

        public ShopRepository(LoyaltyCardContext context)
        {
            _context = context;
        }

        public async Task<Shop?> GetByIdAsync(string shopId)
        {
            return await _context.Shops
                .FirstOrDefaultAsync(s => s.ShopId == shopId);
        }

        public async Task<Shop?> GetByApiKeyAsync(string apiKey)
        {
            return await _context.Shops
                .FirstOrDefaultAsync(s => s.ApiKeyHash == apiKey);
        }

        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            return await _context.Shops.ToListAsync();
        }

        public async Task<Shop> CreateAsync(Shop shop)
        {
            _context.Shops.Add(shop);
            return shop;
        }

        public async Task<Shop> UpdateAsync(Shop shop)
        {
            _context.Shops.Update(shop);
            return shop;
        }

        public async Task DeleteAsync(string shopId)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }
        }

        public async Task<bool> ExistsAsync(string shopId)
        {
            return await _context.Shops.AnyAsync(s => s.ShopId == shopId);
        }

        public async Task<bool> ValidateApiKeyAsync(string shopId, string apiKey)
        {
            return await _context.Shops
                .AnyAsync(s => s.ShopId == shopId && s.ApiKeyHash == apiKey);
        }
    }
}