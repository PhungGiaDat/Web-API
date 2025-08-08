using Web_API.Data;
using Web_API.Models;

namespace Web_API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(string transactionId);
        Task<IEnumerable<Transaction>> GetByCardNumberAsync(string cardNumber);
        Task<IEnumerable<Transaction>> GetByCardNumberPaginatedAsync(string cardNumber, int page, int pageSize);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string cardNumber, int count = 10);
        Task<decimal> GetTotalPointsEarnedAsync(string cardNumber);
        Task<decimal> GetTotalPointsRedeemedAsync(string cardNumber);
        Task<int> GetTotalPointsRedeemedGlobalAsync();
        Task<IEnumerable<TopCustomer>> GetTopCustomersByPointsRedeemedAsync(int limit = 10);
    }
}