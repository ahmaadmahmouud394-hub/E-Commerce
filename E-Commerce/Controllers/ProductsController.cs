using E_Commerce.BusinessObject;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductBO _productBO;

        public ProductsController(ProductBO productBO)
        {
            _productBO = productBO;
        }
        [HttpPost]
        public IActionResult CreateProduct(JsonObject product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            else
            {
                _productBO.GetCreated(product);
                return Ok();
            }
        }
        [HttpGet]
        public List<Product> GetProducts()
        {
            var Products = _productBO.GetProducts();
            return Products;
        }
        [HttpPost]
        public IActionResult GetProduct(JsonObject productid)
        {
            if (productid == null)
            {
                return BadRequest();
            }
            else
            {
                var product = _productBO.GetProductById(productid);
                return Ok(product);
            }
        }
        [HttpPut]
        public IActionResult UpdateProduct(JsonObject productid)
        {
            if (productid == null)
            {
                return BadRequest();
            }
            else
            {
                _productBO.GetUpdated(productid);
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteProduct(JsonObject productid)
        {
            if (productid == null)
            {
                return BadRequest();
            }
            else
            {
                _productBO.GetDeleted(productid);
                return Ok();
            }
        }
    }
}
