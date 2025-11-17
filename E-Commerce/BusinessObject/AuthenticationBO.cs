using E_Commerce.Data;
using E_Commerce.Domain.Entities;

namespace E_Commerce.BusinessObject
{
    public class AuthenticationBO
    {
        private readonly AppDbContext _context;

        public AuthenticationBO(AppDbContext context)
        {
            _context = context;
        }

        public bool Authenticate(User user)
        {

            var IsAuthenticated = _context.Users.Where(e => e.UserName == user.UserName && e.Password == user.Password).Any();
            return IsAuthenticated;
        }
        public string GetAuthenticated(User user) 
        {
            return "";
        }
    }
}
