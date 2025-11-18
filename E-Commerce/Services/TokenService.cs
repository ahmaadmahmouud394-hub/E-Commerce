using E_Commerce.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Services
{
    public class TokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _minutes;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            _issuer = config["Jwt:Issuer"]!;
            _audience = config["Jwt:Audience"]!;
            _minutes = double.Parse(config["Jwt:AccessTokenMinutes"]!);
        }

        // This method is now updated
        public string GenerateJwtToken(User user)
        {
            //var claims = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //};

            //// Add all roles as "role" claims
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.Name));
            //}

            //// Add all unique permissions as "permission" claims
            //foreach (var perm in permissions)
            //{
            //    claims.Add(new Claim("permission", perm.Name));
            //}

            //var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    issuer: _issuer,
            //    audience: _audience,
            //    claims: claims,
            //    expires: DateTime.Now.AddMinutes(_minutes),
            //    signingCredentials: creds);

            //return new JwtSecurityTokenHandler().WriteToken(token);
            return user.Name;
        }
    }
}