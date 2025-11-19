using Microsoft.AspNetCore.Mvc;
using E_Commerce.Services;
using E_Commerce.BusinessObject;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleBO _roleBo;
        public RolesController(RoleBO roleBo)
        {
            _roleBo = roleBo;
        }

        [HttpPost]
        public IActionResult CreateRole([FromBody] CreateRoleDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                int newId = _roleBo.CreateRole(request);
                return CreatedAtAction(nameof(GetRole), new { id = newId }, new { message = "Role created successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleBo.GetRoles());
        }

        [HttpGet]
        public IActionResult GetRole(int id)
        {
            var role = _roleBo.GetRoleById(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // PUT /api/roles
        [HttpPut]
        public IActionResult UpdateRole([FromBody] UpdateRoleDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _roleBo.UpdateRole(request);
                return Ok(new { message = "Role updated successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Role not found");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRole(int id)
        {
            var success = _roleBo.DeleteRole(id);
            if (success) return NoContent();
            return NotFound("Role not found");
        }

    }
}
