using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class RoleBO
    {
        private readonly AppDbContext _context;
        public RoleBO(AppDbContext context)
        {
            _context = context;
        }
        //public void GetCreated(JsonObject user)
        //{
        //    var RoleCreate = new Role();
        //    RoleCreate.Name = user["name"].GetValue<string>();
        //    RoleCreate.Description = user["description"].GetValue<string>();
        //    _context.Users.Add(RoleCreate);
        //    _context.SaveChanges();
        //}
        public List<User> GetUsers()
        {
            var users = new List<User>();
            users = _context.Users.ToList();
            return users;
        }
        public Domain.Entities.User GetUserById(JsonObject id)
        {
            int iduser = id["username"].GetValue<int>();
            var user = _context.Users.Where(a => a.Id == iduser).FirstOrDefault();
            return user;
        }
        public void GetUpdated(JsonObject user)
        {
            var finduser = new Domain.Entities.User();
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

        public bool GetDeleted(JsonObject idjson)
        {
            var id = idjson["id"].GetValue<int>();

            var User = _context.Users.Find(id);

            if (User != null)
            {
                _context.Users.Remove(User);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
