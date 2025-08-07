using Web_API.Data;

namespace Web_API.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid customerId);
        Task<Customer?> GetByCitizenIdAsync(string citizenId);
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(Guid customerId);
        Task<bool> ExistsAsync(Guid customerId);
    }
}