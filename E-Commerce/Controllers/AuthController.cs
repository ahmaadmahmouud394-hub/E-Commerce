using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Services;
using E_Commerce.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(
        AppDbContext context,
        TokenService tokenService,
        IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
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

        // Assign default "Customer" role
        // We assume Role ID 1 is "Customer"
        var customerRole = await _context.Roles.FindAsync(1);
        if (customerRole != null)
        {
            _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = customerRole.Id });
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "User registered successfully as a Customer." });
    }

    [HttpPost]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequestDto login)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == login.UserName);

        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid username or password");
        }

        // Get ALL of the user's roles
        var roleIds = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var roles = await _context.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync();

        // Get ALL permissions from ALL those roles (and make sure they are unique)
        var permissions = await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission)
            .Distinct()
            .ToListAsync();

        var token = _tokenService.GenerateJwtToken(user, roles, permissions);
        return Ok(new { token = token });
    }
}