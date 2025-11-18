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
        public string GenerateJwtToken(int RoleId, int UserId)
        {
            var Header = RoleId.ToString() + "|" + UserId.ToString();
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, Header),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_minutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public TokenDto? ValidateAndGetUsername(string token)
        {
            var tokenDto = new TokenDto();
            
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,

                    ValidateIssuer = true,
                    ValidIssuer = _issuer,

                    ValidateAudience = true,
                    ValidAudience = _audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // ✅ Extract the username (stored in "sub" claim)
                var DecodedToken = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string UserId = DecodedToken.Split("|").Last() ?? "";
                tokenDto.UserId = int.Parse(UserId);
                string Role = DecodedToken.Split("|").First() ?? "";
                tokenDto.RoleId = int.Parse(Role);
                


                return tokenDto;
            }
            catch
            {
                return null;
            }
        }
    }
}