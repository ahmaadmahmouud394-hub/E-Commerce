using E_Commerce.BusinessObject;
using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesBO _CategoriesBO;
        public CategoriesController(CategoriesBO categoriesBO)
        {
            _CategoriesBO = categoriesBO;
        }
        [HttpPost]
        public IActionResult CreateCategory(JsonObject category)
        {
            if (category == null)
            {
                return BadRequest();
            }
            else
            {
                _CategoriesBO.GetCreated(category);
                return Ok();
            }
        }
        [HttpGet]
        public List<Category> GetAllCategories()
        {
            var categories = _CategoriesBO.GetCategories();
            return categories;
        }
        [HttpPost]
        public IActionResult GetCategory(JsonObject CategoryId)
        {
            if (CategoryId == null)
            {
                return BadRequest();
            }
            else
            {
                var Category = _CategoriesBO.GetCategoryById(CategoryId);
                return Ok(Category);
            }
        }
        [HttpPut]
        public IActionResult UpdateCategory(JsonObject CategoryId)
        {
            if (CategoryId == null)
            {
                return BadRequest();
            }
            else
            {
                _CategoriesBO.GetUpdated(CategoryId);
                return Ok();
            }
        }

        [HttpDelete]
        public IActionResult DeleteCategory(JsonObject CategoryId)
        {
            if (CategoryId == null)
            {
                return BadRequest();
            }
            else
            {
                _CategoriesBO.GetDeleted(CategoryId);
                return Ok();
            }

        }
    }
}
