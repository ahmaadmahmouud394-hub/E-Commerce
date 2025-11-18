using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Data.SqlClient;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class AuthenticationBO
    {
        private readonly AppDbContext _context;

        public AuthenticationBO(AppDbContext context)
        {
            _context = context;
        }
        public User GetAuthenticated(JsonObject Auth)
        {
            var username= Auth["username"]?.GetValue<string>();
            var password= Auth["password"]?.GetValue<string>();
            var Email= Auth["email"]?.GetValue<string>();
            if (Email == null) {
                var IsAuthenticated = _context.Users.Where(e => e.UserName == username && e.Password == password).FirstOrDefault();
                return IsAuthenticated;
            }
            else
            {
                var IsAuthenticated = _context.Users.Where(e => e.Email == Email && e.Password == password).FirstOrDefault();
                return IsAuthenticated;
            }
        }
    }
}
