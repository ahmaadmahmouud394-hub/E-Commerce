using E_Commerce.BusinessObject;
using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserBO _userBo;

    public UsersController(UserBO userBO)
    {
        _userBo = userBO;
    }
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
    [HttpGet]
    public List<User> GetUsers()
    {
        var Users = _userBo.GetUsers();
        return Users;
    }
    [HttpPost]
    public IActionResult GetUser(JsonObject userid)
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

    [HttpPut]
    public IActionResult DeleteUser(JsonObject userid)
    {
        if (userid == null)
        {
            return BadRequest();
        }
        else
        {
            _userBo.GetDeleted(userid);
            return Ok();
        }
    }
}