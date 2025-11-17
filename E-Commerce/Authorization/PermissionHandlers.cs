using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Authorization
{
    // This handler checks if the user has the required permission
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // Get all permission claims from the user's token
            var userPermissions = context.User.Claims
                                          .Where(c => c.Type == "permission")
                                          .Select(c => c.Value);

            // Check if any of the user's permissions match any of the required permissions
            if (requirement.Permissions.Any(requiredPermission => userPermissions.Contains(requiredPermission)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    // This Policy Provider dynamically creates policies for permissions
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Check if it's a policy we've already created
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is not null)
                return policy;

            // This policyName is the string from the [HasPermission] attribute
            // e.g., "UserRead" or "UserRead,UserCreate"
            var permissions = policyName.Split(',');

            // Create a new policy that contains our requirement
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permissions))
                .Build();
        }
    }
}