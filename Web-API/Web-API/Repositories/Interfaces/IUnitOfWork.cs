using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        ILoyaltyCardRepository LoyaltyCards { get; }
        ITransactionRepository Transactions { get; }
        IRewardRepository Rewards { get; }
        IShopRepository Shops { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}