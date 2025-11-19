using E_Commerce.Data;
using System.Text.Json.Nodes;
using E_Commerce.Domain.Entities;
using E_Commerce.Services;

namespace E_Commerce.BusinessObject
{
    public class UserBO
    {
        private readonly AppDbContext _context;
        private readonly ImageHandler _imageHandler;
        public UserBO(AppDbContext context,ImageHandler img)
        {
            _context = context;
            _imageHandler = img;
        }
        public void GetCreated(JsonObject user)
        {
            var UserCreate = new User();
            UserCreate.UserName = user["username"]?.GetValue<string>();
            UserCreate.Password = user["password"]?.GetValue<string>();
            UserCreate.Email = user["email"]?.GetValue<string>();
            UserCreate.Address = user["address"]?.GetValue<string>();
            string v = user["birthdate"]?.GetValue<string?>();
            UserCreate.BirthDate = v != null ? DateOnly.Parse(v) : null;
            UserCreate.AvatarUrl = user["avatarurl"]?.GetValue<string>();
            UserCreate.Name = user["name"]?.GetValue<string>();
            UserCreate.RoleId = user["roleid"].GetValue<int>();
            var byteimage = user["image"].GetValue<byte[]>();
            UserCreate.AvatarUrl = _imageHandler.HandledURL(byteimage, "users");
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
            int iduser = id["id"].GetValue<int>();
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
            //UserUpdate.BirthDate = user["birthdate"].GetValue<DateOnly>();
            string v = user["birthdate"]?.GetValue<string?>();
            UserUpdate.BirthDate = v != null ? DateOnly.Parse(v) : null;
            UserUpdate.AvatarUrl = user["avatarurl"].GetValue<string>();
            UserUpdate.Name = user["name"].GetValue<string>();
            var Role = user["role"].GetValue<string>();

            var byteimage = user["image"].GetValue<string>();
            UserUpdate.AvatarUrl = _imageHandler.HandledURL(byteimage, "users");

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
