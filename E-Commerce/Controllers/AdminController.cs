using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using E_Commerce.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AdminController(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("users")]
    //[HasPermission(PermissionsEnum.UserCreate)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Address = request.Address,
            UserName = request.UserName,
            BirthDate = request.BirthDate,
        };

        user.Password = _passwordHasher.HashPassword(user, user.Password);

        if (await _context.Users.AnyAsync(u => u.UserName == user.UserName || u.Email == user.Email))
        {
            return BadRequest("Username or Email already exists.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // Save user to get their new ID

        // Loop through the list of RoleIds from the request and assign them
        foreach (var roleId in request.RoleIds.Distinct()) // Use .Distinct() for safety
        {
            if (await _context.Roles.AnyAsync(r => r.Id == roleId))
            {
                //_context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleId });
            }
        }
        await _context.SaveChangesAsync();

        user.Password = "";
        return Ok(user);
    }
}