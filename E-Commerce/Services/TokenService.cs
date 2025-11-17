using E_Commerce.Domain.Entities;
using Microsoft.Extensions.Configuration; 
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; 
using System.Text;

namespace E_Commerce.Services
{
    public class TokenService
    {
        public string GenerateJwtToken(string UserName)
        {
            var Header = UserName;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, Header),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AhmedMahmoudAbdelkarimAhmedEbrahimKhaleefa123456789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ECommerce.com",
                audience: "ECommerce.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(90),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string? ValidateAndGetUsername(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("AhmedMahmoudAbdelkarimAhmedEbrahimKhaleefa123456789");

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = "ECommerce.com",

                    ValidateAudience = true,
                    ValidAudience = "ECommerce.com",

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var username = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //string Role = username.Split("|").Last() ?? "";

                return username;
            }
            catch
            {
                return null;
            }
        }
    }
}