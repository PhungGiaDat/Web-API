namespace Web_API.Security.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GenerateJwtTokenAsync(string userId, string role);
        Task<bool> ValidateJwtTokenAsync(string token);
        Task<string> GetUserIdFromTokenAsync(string token);
        Task<string> GetUserRoleFromTokenAsync(string token);
    }
}