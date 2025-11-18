using E_Commerce.BusinessObject;
using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Nodes;

[ApiController]
[Route("/api/[controller]")]
public class AuthenticateController : ControllerBase
{
 private readonly AuthenticationBO _authBO;
    private readonly TokenService _tokenService;

   public AuthenticateController(AuthenticationBO authBO, TokenService tokenService)
    {
        _authBO = authBO;
        _tokenService = tokenService;
    }
    [Route("api/create/[controller]")]
    [HttpPost]
    public IActionResult GetAuthenticated(JsonObject UserCredentials)
    {
        if (UserCredentials == null)
        {
            return BadRequest();
        }
        else
        {
            var isAuth =_authBO.GetAuthenticated(UserCredentials);
            if (isAuth == null) { 
            return BadRequest();
            }
            else
            {
                var token = _tokenService.GenerateJwtToken(isAuth);
            }
                return Ok();
        }
    }
}