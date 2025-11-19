using E_Commerce.BusinessObject;
using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AuthenticateController : ControllerBase
{
 private readonly AuthenticationBO _authBO;
    private readonly TokenService _tokenService;

   public AuthenticateController(AuthenticationBO authBO, TokenService tokenService)
    {
        _authBO = authBO;
        _tokenService = tokenService;
    }
    [HttpPost]
    public IActionResult GetAuthenticated(JsonObject userCredentials)
    {
        if (userCredentials == null)
        {
            return BadRequest("Invalid request data.");
        }
        var isAuth = _authBO.GetAuthenticated(userCredentials);
        if (isAuth == null)
        {
            return Unauthorized("Authentication failed. Invalid email or password.");
        }
        else
        {
            var token = _tokenService.GenerateJwtToken(isAuth.Id, isAuth.RoleId);
            return Ok(new { Token = token });
        }
    }
}