using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Repositories.Interfaces;

namespace Web_API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly LoyaltyCardContext _context;

        public CustomerRepository(LoyaltyCardContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid customerId)
        {
            return await _context.Customers
                .Include(c => c.LoyaltyCards)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer?> GetByCitizenIdAsync(string citizenId)
        {
            return await _context.Customers
                .Include(c => c.LoyaltyCards)
                .FirstOrDefaultAsync(c => c.CitizenId == citizenId);
        }

        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Customers
                .Include(c => c.LoyaltyCards)
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return customer;
        }

        public async Task DeleteAsync(Guid customerId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public async Task<bool> ExistsAsync(Guid customerId)
        {
            return await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
        }
    }
}