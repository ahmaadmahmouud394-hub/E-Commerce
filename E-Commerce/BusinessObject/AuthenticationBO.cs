using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class AuthenticationBO
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationBO(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public User? GetAuthenticated(JsonObject Auth)
        {
            var username= Auth["username"]?.GetValue<string>();
            var password= Auth["password"]?.GetValue<string>();
            var email= Auth["email"]?.GetValue<string>();
            User? user = null;

            if (!string.IsNullOrEmpty(email))
            {
                user = _context.Users.FirstOrDefault(e => e.Email == email);
            }
            else if (!string.IsNullOrEmpty(username))
            {
                user = _context.Users.FirstOrDefault(e => e.UserName == username);
            }

            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password ?? "");

            if (result == PasswordVerificationResult.Success)
            {
                return user;
            }

            return null;
        }
    }
}
