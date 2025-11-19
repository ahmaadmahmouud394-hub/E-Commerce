using E_Commerce.Data;
using E_Commerce.Domain.Entities;
using System.Text.Json.Nodes;

namespace E_Commerce.BusinessObject
{
    public class ProductBO
    {
        private readonly AppDbContext _context;
        public ProductBO(AppDbContext context)
        {
            _context = context;
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
            //ProductCreate.WarrantyMaxDate = product["warrantymmaxdate"]?.GetValue<DateOnly>() ?? DateOnly.MinValue;
            string v = product["WarrantyMaxDate"]?.GetValue<string?>();
            ProductCreate.WarrantyMaxDate = v != null ? DateOnly.Parse(v) : DateOnly.MinValue;
            ProductCreate.CategoryId = product["categoryid"]?.GetValue<int>() ?? 0;
            ProductCreate.BrandId = product["brandid"]?.GetValue<int>() ?? 0;


            //adding to db
            _context.Products.Add(ProductCreate);
            _context.SaveChanges();

            var base64Image = product["image"]?.GetValue<string>();

            if (!string.IsNullOrEmpty(base64Image))
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                // Create folder path
                string folderPath = Path.Combine("wwwroot", "images", "products");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Create unique filename
                    string fileName = $"{Guid.NewGuid()}.jpg";
                string filePath = Path.Combine(folderPath, fileName);

                // Save to disk
                File.WriteAllBytes(filePath, imageBytes);

                // Save URL in DB
                var facility = new Facility
                {
                    Alt = "product-image",
                    ImageUrl = $"{folderPath}/{fileName}",
                    ProductId = ProductCreate.Id
                };

                _context.Facilities.Add(facility);
                _context.SaveChanges();
            }
        }
        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            products = _context.Products.ToList();
            return products;
        }
        public Domain.Entities.Product GetProductById(JsonObject id)
        {
            int idproduct = id["id"].GetValue<int>();
            var product = _context.Products.Where(a => a.Id == idproduct).FirstOrDefault();
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
            //ProductUpdate.WarrantyMaxDate = product["warrantymmaxdate"]?.GetValue<DateOnly>() ?? DateOnly.MinValue;
            string v = product["WarrantyMaxDate"]?.GetValue<string?>();
            ProductUpdate.WarrantyMaxDate = v != null ? DateOnly.Parse(v) : DateOnly.MinValue;
            ProductUpdate.CategoryId = product["categoryid"]?.GetValue<int>() ?? 0;
            ProductUpdate.BrandId = product["brandid"]?.GetValue<int>() ?? 0;

            //adding to db
            _context.Products.Update(ProductUpdate);
            _context.SaveChanges();
        }

        public bool GetDeleted(JsonObject idjson)
        {
            var id = idjson["id"].GetValue<int>();

            var Product = _context.Products.Find(id);

            if (Product != null)
            {
                _context.Products.Remove(Product);
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
