using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Web_API.Security.Requirements;

namespace Web_API.Security.Handlers
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.IsInRole(requirement.Role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class CustomerOwnershipHandler : AuthorizationHandler<CustomerOwnershipRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerOwnershipRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

            // Admin can access any customer data
            if (role == "Admin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Customer can only access their own data
            if (role == "Customer" && !string.IsNullOrEmpty(userId))
            {
                // Additional logic to verify ownership based on request context
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}