using Microsoft.EntityFrameworkCore.Storage;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoyaltyCardContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(LoyaltyCardContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            LoyaltyCards = new LoyaltyCardRepository(_context);
            Transactions = new TransactionRepository(_context);
            Rewards = new RewardRepository(_context);
            Shops = new ShopRepository(_context);
        }

        public ICustomerRepository Customers { get; }
        public ILoyaltyCardRepository LoyaltyCards { get; }
        public ITransactionRepository Transactions { get; }
        public IRewardRepository Rewards { get; }
        public IShopRepository Shops { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}