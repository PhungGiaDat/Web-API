using Web_API.Data;

namespace Web_API.Repositories.Interfaces
{
    public interface ILoyaltyCardRepository
    {
        Task<LoyaltyCard?> GetByCardNumberAsync(string cardNumber);
        Task<LoyaltyCard?> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<LoyaltyCard>> GetByCustomerIdListAsync(Guid customerId);
        Task<LoyaltyCard> CreateAsync(LoyaltyCard loyaltyCard);
        Task<LoyaltyCard> UpdateAsync(LoyaltyCard loyaltyCard);
        Task DeleteAsync(string cardNumber);
        Task<bool> ExistsAsync(string cardNumber);
        Task<bool> UpdatePointsAsync(string cardNumber, int pointsChange);
        Task<int> GetTotalCardsCountAsync();
    }
}