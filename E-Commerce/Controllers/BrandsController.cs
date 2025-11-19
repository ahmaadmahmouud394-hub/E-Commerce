using E_Commerce.BusinessObject;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly BrandBO _brandBO;
        public BrandsController(BrandBO brandBO)
        {
            _brandBO = brandBO;
        }
        [HttpPost]
        public IActionResult CreateBrand(JsonObject brand)
        {
            if (brand == null)
            {
                return BadRequest();
            }
            else
            {
                _brandBO.GetCreated(brand);
                return Ok();
            }
        }
        [HttpGet]
        public List<Brand> GetAllBrands()
        {
            var Brands = _brandBO.GetBrands();
            return Brands;
        }
        [HttpPost]
        public IActionResult GetBrand(JsonObject BrandId)
        {
            if (BrandId == null)
            {
                return BadRequest();
            }
            else
            {
                var Brand = _brandBO.GetBrandById(BrandId);
                return Ok(Brand);
            }
        }
        [HttpPut]
        public IActionResult UpdateBrand(JsonObject brandId)
        {
            if (brandId == null)
            {
                return BadRequest();
            }
            else
            {
                _brandBO.GetUpdated(brandId);
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteBrand(JsonObject BrandId)
        {
            if (BrandId == null)
            {
                return BadRequest();
            }
            else
            {
                _brandBO.GetDeleted(BrandId);
                return Ok();
            }

        }
    }
}

