using Web_API.Models;

namespace Web_API.Services.Interfaces
{
    public interface ISystemService
    {
        Task<SystemStatisticsResponse> GetSystemStatisticsAsync();
    }
}