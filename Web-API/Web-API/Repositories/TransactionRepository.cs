using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly LoyaltyCardContext _context;

        public TransactionRepository(LoyaltyCardContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(string transactionId)
        {
            return await _context.Transactions
                .Include(t => t.CardNumberNavigation)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetByCardNumberAsync(string cardNumber)
        {
            return await _context.Transactions
                .Include(t => t.CardNumberNavigation)
                .Where(t => t.CardNumber == cardNumber)
                .OrderByDescending(t => t.TransactionTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByCardNumberPaginatedAsync(string cardNumber, int page, int pageSize)
        {
            return await _context.Transactions
                .Include(t => t.CardNumberNavigation)
                .Where(t => t.CardNumber == cardNumber)
                .OrderByDescending(t => t.TransactionTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string cardNumber, int count = 10)
        {
            return await _context.Transactions
                .Include(t => t.CardNumberNavigation)
                .Where(t => t.CardNumber == cardNumber)
                .OrderByDescending(t => t.TransactionTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPointsEarnedAsync(string cardNumber)
        {
            return await _context.Transactions
                .Where(t => t.CardNumber == cardNumber && t.PointsChanged > 0)
                .SumAsync(t => t.PointsChanged);
        }

        public async Task<decimal> GetTotalPointsRedeemedAsync(string cardNumber)
        {
            return await _context.Transactions
                .Where(t => t.CardNumber == cardNumber && t.PointsChanged < 0)
                .SumAsync(t => Math.Abs(t.PointsChanged));
        }
    }
}