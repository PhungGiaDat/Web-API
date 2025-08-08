using Web_API.Services.Interfaces;
using Web_API.Repositories.Interfaces;
using Web_API.Models;

namespace Web_API.Services
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SystemStatisticsResponse> GetSystemStatisticsAsync()
        {
            try
            {
                // Get total cards count
                var totalCards = await _unitOfWork.LoyaltyCards.GetTotalCardsCountAsync();

                // Get total points redeemed globally
                var totalPointsRedeemed = await _unitOfWork.Transactions.GetTotalPointsRedeemedGlobalAsync();

                // Get top customers by points redeemed
                var topCustomers = await _unitOfWork.Transactions.GetTopCustomersByPointsRedeemedAsync(10);

                return new SystemStatisticsResponse
                {
                    TotalCards = totalCards,
                    TotalPointsRedeemed = totalPointsRedeemed,
                    TopCustomers = topCustomers.ToList()
                };
            }
            catch (Exception)
            {
                // Return empty statistics in case of error
                return new SystemStatisticsResponse
                {
                    TotalCards = 0,
                    TotalPointsRedeemed = 0,
                    TopCustomers = new List<TopCustomer>()
                };
            }
        }
    }
}