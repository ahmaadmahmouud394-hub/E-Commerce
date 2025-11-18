using E_Commerce.BusinessObject;
using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace E_Commerce.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        //private readonly AuthenticationBO _authentication;
        //private readonly AppDbContext _appDbContext;
        public Middleware(RequestDelegate next)
        {
            _next = next;
            //_appDbContext = appDbContext;
        }
        public async Task InvokeAsync(HttpContext context, AppDbContext _appDbContext)
        {
            var path = context.Request.Path.Value?.ToLower().TrimEnd('/');
            var method = context.Request.Method.ToUpper();

            if (path != null && (path.EndsWith("authenticate/getauthenticated")))
            {
                await _next(context);
                return;
            }

            string referer = "?";
            if (context.Request.Method.ToLower() != "get")
            {
                Uri.TryCreate(context.Request.Headers.Referer.ToString(), new UriCreationOptions(), out var res);
                referer = res?.AbsolutePath ?? "/";
            }

            if (path == "/" && (referer) != "/")
            {
                path = referer.ToLower();
            }
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                               context.User.FindFirst("sub")?.Value;

            var roleIdString = context.User.FindFirst("RoleId")?.Value;

            if (string.IsNullOrEmpty(userIdString) || string.IsNullOrEmpty(roleIdString))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!int.TryParse(roleIdString, out int roleId))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            //var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            //if (role == null)
            //{
            //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    return;
            //}
            string requiredPermission = "";
            string resource = "";
            if (path.ToLower().StartsWith("/api/user")) resource = "User";
            else if (path.ToLower().StartsWith("/api/role")) resource = "Role";
            else if (path.ToLower().StartsWith("/api/product")) resource = "Product";
            else if (path.ToLower().StartsWith("/api/brand")) resource = "Brand";
            else if (path.ToLower().StartsWith("/api/category")) resource = "Category";

            if (!string.IsNullOrEmpty(resource))
            {
                // Map HTTP Methods to Actions
                if (method == "GET") requiredPermission = $"Read-{resource}";
                else if (method == "POST") requiredPermission = $"Write-{resource}"; // Create
                else if (method == "PUT" || method == "PATCH") requiredPermission = $"Write-{resource}"; // Update
                else if (method == "DELETE") requiredPermission = $"Delete-{resource}";

                // Fallback: If you use custom routes like /api/user/delete/1 using POST
                if (path.Contains($"/delete")) requiredPermission = $"Delete-{resource}";
                if (path.Contains($"/create") || path.Contains($"/update")) requiredPermission = $"Write-{resource}";
            }

            if (!string.IsNullOrEmpty(requiredPermission))
            {
                var hasPermission = await _appDbContext.RolePermissions
                    .Include(rp => rp.Permission)
                    .AnyAsync(rp => rp.RoleId == roleId && rp.Permission.Name == requiredPermission);
                if (!hasPermission)
                {
                    // User is Logged In, but does not have permission
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    // Optional: For debugging, you can uncomment the line below
                    // await context.Response.WriteAsJsonAsync(new { error = $"Forbidden. Missing: {requiredPermission}" });
                    return;
                }
                await _next(context);
            }

                ////var token = context.Session.GetString("token");
                ////------------------------------------checking permessions----------------------------------
                //if (path.ToLower().Contains("/delete") || path.ToLower().Contains("/update") || path.ToLower().Contains("/Create") || path.ToLower().Contains("/"))
                //{
                //    //------------------Users
                //    //------Delete ------ Users
                //    if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/users"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Delete-Users"
                //                             select rp).AnyAsync();
                //        if (hasPermission) 
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Write ------ Users
                //    if ((path.ToLower().Contains("/update")|| path.ToLower().Contains("/create")) && path.ToLower().Contains("/users"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Write-Users"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Read ------ Users
                //    if (path.ToLower().Contains("/") && path.ToLower().Contains("/users"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Read-Users"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------------------------------------products
                //    //------Delete ------ Products
                //    if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/products"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Delete-Products"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Write ------ Products
                //    if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/products"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Write-Products"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Read ------ Products
                //    if (path.ToLower().Contains("/") && path.ToLower().Contains("/products"))
                //    {
                //        var hasPermission =  await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Read-Products"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //--------------------------------------Brands
                //    //------Delete ------ Brands
                //    if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/brands"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Delete-Brands"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Write ------ Brands
                //    if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/brands"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Write-Brands"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //----- Read ------ Brands
                //    if (path.ToLower().Contains("/") && path.ToLower().Contains("/brands"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Read-Brands"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //--------------------------------------Categories
                //    //-----Delete-----Categories
                //    if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/categories"))
                //    {
                //        var hasPermission =  await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Delete-Categories"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Write ------ Categories
                //    if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/categories"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Write-Categories"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //----- Read ------ Categories
                //    if (path.ToLower().Contains("/") && path.ToLower().Contains("/categories"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Read-Categories"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //--------------------------------------Roles
                //    //-----Delete-----Roles
                //    if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/roles"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Delete-Roles"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //------Write ------ Roles
                //    if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/roles"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Write-Roles"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    //----- Read ------ Roles
                //    if (path.ToLower().Contains("/read") && path.ToLower().Contains("/roles"))
                //    {
                //        var hasPermission = await (from rp in _appDbContext.RolePermissions
                //                             join p in _appDbContext.Permissions
                //                             on rp.PermissionId equals p.Id
                //                             where rp.RoleId == role.Id && p.Name == "Read-Roles"
                //                             select rp).AnyAsync();
                //        if (hasPermission)
                //        {
                //            await _next(context);
                //            return;
                //        }
                //        else
                //        {
                //            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //        return;
                //    }
                //}


                // --- 2. Authentication Check ---

                // Optional: Re-validate token if it's missing but UserId is present
                //if (string.IsNullOrEmpty(token))
                //{
                //    // Re-authenticate logic using the ID we have from session
                //    if (Int32.TryParse(userIdString, out int ID))
                //    {
                //        var user = new User();
                //        user.Id = ID;
                //        // This is likely calling a service to get the user/token and set session again
                //        var userAuth = authBO.GetAuthenticated(user);
                //        // After re-authentication, you should typically check if it succeeded 
                //        // and maybe re-set session variables if they changed.
                //    }
                //    else
                //    {
                //        // If UserId is present but invalid/unparseable, redirect to sign in
                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //        return;
                //    }
                //}
                // The rest of the requests (authenticated and authorized) proceed
                await _next(context);
        }
    }
}
