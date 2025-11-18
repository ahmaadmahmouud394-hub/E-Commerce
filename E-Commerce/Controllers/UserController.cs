using E_Commerce.BusinessObject;
using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly UserBO _userBo;

    public UserController(AppDbContext context, IPasswordHasher<User> passwordHasher,UserBO userBO)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    [Route("api/create/[controller]")]
    [HttpPost]
    public IActionResult CreateUser(JsonObject user)
    {
        if (user == null) { 
            return BadRequest();
        }
        else
        {
            _userBo.GetCreated(user);
            return Ok();
        }
    }
    [Route("api/Get/[controller]")]
    [HttpGet]
    public List<User> GetUser()
    {
        var Users = _userBo.GetUsers();
        return Users;
    }
    [Route("api/Get/Update/[controller]")]
    [HttpPost]
    public IActionResult GetUpdateUser(JsonObject userid)
    {
        if (userid == null)
        {
            return BadRequest();
        }
        else
        {
            var user = _userBo.GetUserById(userid);
            return Ok(user);
        }
    }
    [Route("api/Update/[controller]")]
    [HttpPut]
    public IActionResult UpdateUser(JsonObject userid)
    {
        if (userid == null)
        {
            return BadRequest();
        }
        else
        {
             _userBo.GetUpdated(userid);
            return Ok();
        }
    }

}