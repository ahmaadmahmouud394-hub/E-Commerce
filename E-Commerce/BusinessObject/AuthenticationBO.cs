using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Data.SqlClient;

namespace E_Commerce.BusinessObject
{
    public class AuthenticationBO
    {
        private readonly AppDbContext _context;

        public AuthenticationBO(AppDbContext context)
        {
            _context = context;
        }
        public User GetAuthenticated(User user)
        {
            var IsAuthenticated = _context.Users.Where(e => e.UserName == user.UserName && e.Password == user.Password).FirstOrDefault();
            return IsAuthenticated;
        }
    }
}
