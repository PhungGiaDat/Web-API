using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class LoyaltyCardRepository : ILoyaltyCardRepository
    {
        private readonly LoyaltyCardContext _context;

        public LoyaltyCardRepository(LoyaltyCardContext context)
        {
            _context = context;
        }

        public async Task<LoyaltyCard?> GetByCardNumberAsync(string cardNumber)
        {
            return await _context.LoyaltyCards
                .Include(lc => lc.Customer)
                .Include(lc => lc.Transactions)
                .FirstOrDefaultAsync(lc => lc.CardNumber == cardNumber);
        }

        public async Task<LoyaltyCard?> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.LoyaltyCards
                .Include(lc => lc.Customer)
                .Include(lc => lc.Transactions)
                .FirstOrDefaultAsync(lc => lc.CustomerId == customerId);
        }

        public async Task<IEnumerable<LoyaltyCard>> GetByCustomerIdListAsync(Guid customerId)
        {
            return await _context.LoyaltyCards
                .Include(lc => lc.Customer)
                .Include(lc => lc.Transactions)
                .Where(lc => lc.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<LoyaltyCard> CreateAsync(LoyaltyCard loyaltyCard)
        {
            _context.LoyaltyCards.Add(loyaltyCard);
            return loyaltyCard;
        }

        public async Task<LoyaltyCard> UpdateAsync(LoyaltyCard loyaltyCard)
        {
            _context.LoyaltyCards.Update(loyaltyCard);
            return loyaltyCard;
        }

        public async Task DeleteAsync(string cardNumber)
        {
            var loyaltyCard = await _context.LoyaltyCards
                .FirstOrDefaultAsync(lc => lc.CardNumber == cardNumber);
            
            if (loyaltyCard != null)
            {
                _context.LoyaltyCards.Remove(loyaltyCard);
            }
        }

        public async Task<bool> ExistsAsync(string cardNumber)
        {
            return await _context.LoyaltyCards
                .AnyAsync(lc => lc.CardNumber == cardNumber);
        }

        public async Task<bool> UpdatePointsAsync(string cardNumber, int pointsChange)
        {
            var loyaltyCard = await _context.LoyaltyCards
                .FirstOrDefaultAsync(lc => lc.CardNumber == cardNumber);
            
            if (loyaltyCard != null)
            {
                loyaltyCard.AvailablePoints += pointsChange;
                return true;
            }
            
            return false;
        }
    }
}