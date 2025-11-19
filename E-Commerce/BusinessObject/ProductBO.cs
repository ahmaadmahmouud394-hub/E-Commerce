using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using E_Commerce.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class ProductBO
    {
        private readonly AppDbContext _context;
        private readonly ImageHandler _imageHandler;
        public ProductBO(AppDbContext context,ImageHandler imageHandler)
        {
            _context = context;
            _imageHandler = imageHandler;
        }
        public void GetCreated(JsonObject product)
        {
            var ProductCreate = new Product();

            ProductCreate.Name = product["name"]?.GetValue<string>();
            ProductCreate.Description = product["description"]?.GetValue<string>();
            ProductCreate.Price = product["price"]?.GetValue<decimal>() ?? 0;
            ProductCreate.Quantity = product["quantity"]?.GetValue<int>() ?? 0;
            ProductCreate.InWarranty = product["inwarranty"]?.GetValue<bool>() ?? false;
            ProductCreate.WarrantyDescription = product["warrantydescription"]?.GetValue<string>();
            ProductCreate.WarrantyMaxDate = product["warrantymmaxdate"]?.GetValue<DateOnly>() ?? DateOnly.MinValue;
            ProductCreate.CategoryId = product["categoryid"]?.GetValue<int>() ?? 0;
            ProductCreate.BrandId = product["brandid"]?.GetValue<int>() ?? 0;


            //adding to db
            _context.Products.Add(ProductCreate);
            _context.SaveChanges();

            var base64Image = product["image"]?.GetValue<string>();

            
                // Save Facility
                var facility = new Facility
                {
                    Alt = "product-image",
                    ImageUrl = _imageHandler.HandledURL(base64Image,"products"),
                    ProductId = ProductCreate.Id
                };

                _context.Facilities.Add(facility);
                _context.SaveChanges();
            
        }
        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            products = _context.Products
                .Include(x => x.CategoryId)
                .Include(x => x.BrandId)
                .Include(x=>x.Facilities)
                .ToList();
            return products;
        }
        public Product GetProductById(JsonObject id)
        {
            int idproduct = id["name"].GetValue<int>();
            var product = _context.Products.Where(a => a.Id == idproduct)
                .Include(x => x.CategoryId)
                .Include(x => x.BrandId)
                .Include(x => x.Facilities)
                .FirstOrDefault();
            return product;
        }
        public void GetUpdated(JsonObject product)
        {
            var findproduct = new Domain.Entities.Product();
            findproduct.Id = product["id"].GetValue<int>();
            var ProductUpdate = _context.Products.Where(a => a.Id == findproduct.Id).FirstOrDefault();
            ProductUpdate.Name = product["name"]?.GetValue<string>();
            ProductUpdate.Description = product["description"]?.GetValue<string>();
            ProductUpdate.Price = product["price"]?.GetValue<decimal>() ?? 0;
            ProductUpdate.Quantity = product["quantity"]?.GetValue<int>() ?? 0;
            ProductUpdate.InWarranty = product["inwarranty"]?.GetValue<bool>() ?? false;
            ProductUpdate.WarrantyDescription = product["warrantydescription"]?.GetValue<string>();
            ProductUpdate.WarrantyMaxDate = product["warrantymmaxdate"]?.GetValue<DateOnly>() ?? DateOnly.MinValue;
            ProductUpdate.CategoryId = product["categoryid"]?.GetValue<int>() ?? 0;
            ProductUpdate.BrandId = product["brandid"]?.GetValue<int>() ?? 0;

            //adding to db
            _context.Products.Update(ProductUpdate);
            _context.SaveChanges();
            var base64Image = product["image"]?.GetValue<string>();


            // Save Facility
            var facility = new Facility
            {
                Alt = "product-image",
                ImageUrl = _imageHandler.HandledURL(base64Image, "products"),
                ProductId = ProductUpdate.Id
            };

            _context.Facilities.Add(facility);
            _context.SaveChanges();
        }

        public bool GetDeleted(JsonObject idjson)
        {
            var id = idjson["id"].GetValue<int>();

            var Product = _context.Products.Find(id);
            var images = _context.Facilities.Where(a=>a.ProductId == id).ToList();

            if (Product != null)
            {
                _context.Products.Remove(Product);
                _context.Facilities.RemoveRange(images);
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
