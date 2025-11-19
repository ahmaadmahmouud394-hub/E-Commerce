using E_Commerce.Data;
using E_Commerce.Domain.DTO;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.BusinessObject
{
    public class RoleBO
    {
        private readonly AppDbContext _context;

        public RoleBO(AppDbContext context)
        {
            _context = context;
        }

        public int CreateRole(CreateRoleDto request)
        {
            if (_context.Roles.Any(r => r.Name == request.Name))
                throw new InvalidOperationException($"Role '{request.Name}' already exists.");

            var newRole = new Role
            {
                Name = request.Name,
                Description = request.Description,
            };

            _context.Roles.Add(newRole);
            _context.SaveChanges();

            if (request.PermissionIds.Any())
            {
                var newPermissions = request.PermissionIds
                    .Select(permId => new RolePermission { RoleId = newRole.Id, PermissionId = permId })
                    .ToList();

                _context.RolePermissions.AddRange(newPermissions);
                _context.SaveChanges();
            }
            return newRole.Id;
        }


        public List<RoleResponseDto> GetRoles()
        {
            return _context.Roles
                .Include(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission)
                .Select(r => new RoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Permissions = r.RolePermissions!
                        .Select(rp => rp.Permission!.Name)
                        .ToList()
                })
                .ToList();
        }

        public RoleResponseDto? GetRoleById(int id)
        {
            var role = _context.Roles
                .Where(r => r.Id == id)
                .Include(r => r.RolePermissions!)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            if (role == null) return null;

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Permissions = role.RolePermissions!
                    .Select(rp => rp.Permission!.Name)
                    .ToList()
            };
        }

        public void UpdateRole(UpdateRoleDto request)
        {
            var role = _context.Roles.Include(r => r.RolePermissions).FirstOrDefault(r => r.Id == request.Id);

            if (role == null) throw new KeyNotFoundException($"Role with ID {request.Id} not found.");

            role.Name = request.Name;
            role.Description = request.Description;


            _context.RolePermissions.RemoveRange(role.RolePermissions);

            if (request.PermissionIds.Any())
            {
                var newPermissions = request.PermissionIds
                    .Select(permId => new RolePermission { RoleId = role.Id, PermissionId = permId })
                    .ToList();

                _context.RolePermissions.AddRange(newPermissions);
            }

            _context.SaveChanges();
        }

        public bool DeleteRole(int id)
        {
            var role = _context.Roles.Find(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}