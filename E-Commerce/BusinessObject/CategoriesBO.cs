using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class CategoriesBO
    {
        private readonly AppDbContext _context; 
        public CategoriesBO(AppDbContext context)
        {
            _context = context;
        }
        public void GetCreated(JsonObject Category)
        {
            var CategoryCreate = new Category();
            CategoryCreate.Name = Category["name"].GetValue<string>();
            CategoryCreate.Description = Category["description"].GetValue<string>();
            //adding to db
            _context.Categories.Add(CategoryCreate);
            _context.SaveChanges();
        }
        public List<Category> GetCategories()
        {
            var Categories = new List<Category>();
            Categories = _context.Categories.ToList();
            return Categories;
        }
        public Category GetCategoryById(JsonObject id)
        {
            int idCategory = id["id"].GetValue<int>();
            var Category = _context.Categories.Find(idCategory);
            return Category;
        }
        public void GetUpdated(JsonObject Category)
        {
            var CategoryId = Category["id"].GetValue<int>();
            var CategoryUpdate = _context.Categories.Find(CategoryId);
            CategoryUpdate.Name = Category["name"].GetValue<string>();
            CategoryUpdate.Description = Category["description"].GetValue<string>();
            _context.Categories.Update(CategoryUpdate);
            _context.SaveChanges();
        }

        public bool GetDeleted(JsonObject idjson)
        {
            var id = idjson["id"].GetValue<int>();

            var Category = _context.Categories.Find(id);

            if (Category != null)
            {
                _context.Categories.Remove(Category);
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
