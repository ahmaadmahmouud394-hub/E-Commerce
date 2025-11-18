using E_Commerce.Data;
using System.Text.Json.Nodes;
using E_Commerce.Domain.Entities;

namespace E_Commerce.BusinessObject
{
    public class UserBO
    {
        private readonly AppDbContext _context;
        public UserBO(AppDbContext context)
        {
            _context = context;
        }
        public void GetCreated(JsonObject user)
        {
            var UserCreate = new User();
            UserCreate.UserName = user["username"].GetValue<string>();
            UserCreate.Password = user["password"].GetValue<string>();
            UserCreate.Email = user["email"].GetValue<string>();
            UserCreate.Address = user["address"].GetValue<string>();
            UserCreate.BirthDate = user["birthdate"].GetValue<DateOnly>();
            UserCreate.AvatarUrl = user["avatarurl"].GetValue<string>();
            UserCreate.Name = user["name"].GetValue<string>();
            var Role = user["role"].GetValue<string>();
            var role= _context.Roles.Where(a=>a.Name == Role).FirstOrDefault();
            UserCreate.RoleId = role.Id;
            //adding to db
            _context.Users.Add(UserCreate);
            _context.SaveChanges();
        }
        public List<User> GetUsers() { 
            var users = new List<User>();
            users = _context.Users.ToList();
            return users;
        }
        public User GetUserById(JsonObject id) 
        {
            int iduser = id["username"].GetValue<int>();
            var user = _context.Users.Where(a=>a.Id == iduser).FirstOrDefault();
            return user;
        }
        public void GetUpdated(JsonObject user)
        {
            var finduser = new User();
            finduser.Id = user["id"].GetValue<int>();
            var UserUpdate = _context.Users.Where(a => a.Id == finduser.Id).FirstOrDefault();
            UserUpdate.UserName = user["username"].GetValue<string>();
            UserUpdate.Password = user["password"].GetValue<string>();
            UserUpdate.Email = user["email"].GetValue<string>();
            UserUpdate.Address = user["address"].GetValue<string>();
            UserUpdate.BirthDate = user["birthdate"].GetValue<DateOnly>();
            UserUpdate.AvatarUrl = user["avatarurl"].GetValue<string>();
            UserUpdate.Name = user["name"].GetValue<string>();
            var Role = user["role"].GetValue<string>();
            var role = _context.Roles.Where(a => a.Name == Role).FirstOrDefault();
            UserUpdate.RoleId = role.Id;
            //adding to db
            _context.Users.Update(UserUpdate);
            _context.SaveChanges();
        }
    }
}
