using Microsoft.AspNetCore.Authorization;

namespace E_Commerce.Authorization
{
    // This class is the requirement our handler will check
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] Permissions { get; }

        public PermissionRequirement(string[] permissions)
        {
            Permissions = permissions;
        }
    }
}