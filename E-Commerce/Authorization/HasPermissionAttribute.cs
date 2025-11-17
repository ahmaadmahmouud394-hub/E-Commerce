using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

// This is the custom attribute we will use on our controllers
// e.g. [HasPermission(PermissionsEnum.UserRead)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(params PermissionsEnum[] permissions)
        : base(policy: string.Join(",", permissions.Select(p => p.ToString())))
    {
    }
}