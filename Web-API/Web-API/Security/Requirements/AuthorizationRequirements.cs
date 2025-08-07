using Microsoft.AspNetCore.Authorization;

namespace Web_API.Security.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }

    public class CustomerOwnershipRequirement : IAuthorizationRequirement
    {
        // Ensures customer can only access their own data
    }

    public class AdminRequirement : IAuthorizationRequirement
    {
        // Admin-only operations
    }
}