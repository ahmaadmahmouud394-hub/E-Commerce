using E_Commerce.BusinessObject;
using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Data;

namespace E_Commerce.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationBO _authentication;
        private readonly AppDbContext _appDbContext;
        public Middleware(RequestDelegate next,AppDbContext appDbContext)
        {
            _next = next;
            _appDbContext = appDbContext;
        }
        public async Task InvokeAsync(HttpContext context, AuthenticationBO authBO)
        {
            var path = context.Request.Path.Value?.ToLower();

            // --- 1. Allow access to public/unrestricted paths, including authentication pages and static files ---
            if (path != null && (path.StartsWith("/api")))
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
            var userIdString = context.Session.GetString("UserId");
            var roleName = context.Session.GetString("Role");
            var Role = _appDbContext.Roles.Where(a => a.Name == roleName).FirstOrDefault();
            var token = context.Session.GetString("token");
            //------------------------------------checking permessions----------------------------------
            if (path.ToLower().Contains("/delete") || path.ToLower().Contains("/update") || path.ToLower().Contains("/Create") || path.ToLower().Contains("/"))
            {
                //------------------Users
                //------Delete ------ Users
                if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/users"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Delete-Users"
                                         select rp).Any();
                    if (hasPermission) 
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Write ------ Users
                if ((path.ToLower().Contains("/update")|| path.ToLower().Contains("/create")) && path.ToLower().Contains("/users"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Write-Users"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Read ------ Users
                if (path.ToLower().Contains("/") && path.ToLower().Contains("/users"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Read-Users"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------------------------------------products
                //------Delete ------ Products
                if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/products"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Delete-Products"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Write ------ Products
                if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/products"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Write-Products"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Read ------ Products
                if (path.ToLower().Contains("/") && path.ToLower().Contains("/products"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Read-Products"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //--------------------------------------Brands
                //------Delete ------ Brands
                if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/brands"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Delete-Brands"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Write ------ Brands
                if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/brands"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Write-Brands"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //----- Read ------ Brands
                if (path.ToLower().Contains("/") && path.ToLower().Contains("/brands"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Read-Brands"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //--------------------------------------Categories
                //-----Delete-----Categories
                if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/categories"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Delete-Categories"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Write ------ Categories
                if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/categories"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Write-Categories"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //----- Read ------ Categories
                if (path.ToLower().Contains("/") && path.ToLower().Contains("/categories"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Read-Categories"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //--------------------------------------Roles
                //-----Delete-----Roles
                if (path.ToLower().Contains("/delete") && path.ToLower().Contains("/roles"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Delete-Roles"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //------Write ------ Roles
                if ((path.ToLower().Contains("/update") || path.ToLower().Contains("/create")) && path.ToLower().Contains("/roles"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Write-Roles"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                //----- Read ------ Roles
                if (path.ToLower().Contains("/read") && path.ToLower().Contains("/roles"))
                {
                    var hasPermission = (from rp in _appDbContext.RolePermissions
                                         join p in _appDbContext.Permissions
                                         on rp.PermissionId equals p.Id
                                         where rp.RoleId == Role.Id && p.Name == "Read-Roles"
                                         select rp).Any();
                    if (hasPermission)
                    {
                        await _next(context);
                        return;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }


            // --- 2. Authentication Check ---

            if (string.IsNullOrEmpty(userIdString))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Optional: Re-validate token if it's missing but UserId is present
            if (string.IsNullOrEmpty(token))
            {
                // Re-authenticate logic using the ID we have from session
                if (Int32.TryParse(userIdString, out int ID))
                {
                    var user = new Domain.Entities.User();
                    user.Id = ID;
                    // This is likely calling a service to get the user/token and set session again
                    var userAuth = authBO.GetAuthenticated(user);
                    // After re-authentication, you should typically check if it succeeded 
                    // and maybe re-set session variables if they changed.
                }
                else
                {
                    // If UserId is present but invalid/unparseable, redirect to sign in
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            // The rest of the requests (authenticated and authorized) proceed
            await _next(context);
        }
    }
}
