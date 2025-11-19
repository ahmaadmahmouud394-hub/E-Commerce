using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Services;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class BrandBO
    {
        private readonly AppDbContext _context;
        private readonly ImageHandler _imageHandler;
        public BrandBO(AppDbContext context,ImageHandler imageHandler)
        {
            _context = context;
            _imageHandler = imageHandler;
        }
        public void GetCreated(JsonObject brand)
        {
            var BrandCreate = new Brand();
            BrandCreate.Name = brand["name"].GetValue<string>();
            BrandCreate.Description= brand["description"].GetValue<string>();
            BrandCreate.Slogan = brand["slogan"]?.GetValue<string>();
            var image64base = brand["photourl"].GetValue<string>();
            BrandCreate.PhotoUrl = _imageHandler.HandledURL(image64base,"brands");
            //adding to db
            _context.Brands.Add(BrandCreate);
            _context.SaveChanges();
        }
        public List<Brand> GetBrands()
        {
            var Brands = new List<Brand>();
            Brands = _context.Brands.ToList();
            return Brands;
        }
        public Brand GetBrandById(JsonObject id)
        {
            int idBrand = id["id"].GetValue<int>();
            var Brand = _context.Brands.Where(a => a.Id == idBrand).FirstOrDefault();
            return Brand;
        }
        public void GetUpdated(JsonObject brand)
        {
            var BrandId = brand["id"].GetValue<int>();
            var BrandUpdate = _context.Brands.Find(BrandId);
            BrandUpdate.Name = brand["name"].GetValue<string>();
            BrandUpdate.Description = brand["description"].GetValue<string>();
            BrandUpdate.Slogan = brand["slogan"]?.GetValue<string>();
            BrandUpdate.PhotoUrl = brand["photourl"].GetValue<string>();
            _context.Brands.Update(BrandUpdate);
            _context.SaveChanges();
        }

        public bool GetDeleted(JsonObject idjson)
        {
            var id = idjson["id"].GetValue<int>();

            var Brand = _context.Brands.Find(id);

            if (Brand != null)
            {
                _context.Brands.Remove(Brand);
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
