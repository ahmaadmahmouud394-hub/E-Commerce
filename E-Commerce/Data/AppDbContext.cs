using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var permissionsToSeed = new List<Permission>
            {
                // User Permissions
                new Permission { Id = 1, Name = "Read-User", Description = "Permission to Read a user" },
                new Permission { Id = 2, Name = "Write-User", Description = "Permission to create/Edit a user" },
                new Permission { Id = 3, Name = "Delete-User", Description = "Permission to Delete a user" },
                // Role Permissions
                new Permission { Id = 4, Name = "Read-Role", Description = "Permission to Read a role" },
                new Permission { Id = 5, Name = "Write-Role", Description = "Permission to create/Edit a role" },
                new Permission { Id = 6, Name = "Delete-Role", Description = "Permission to Delete a role" },
                //product Permissions
                new Permission { Id = 7, Name = "Read-Product", Description = "Permission to Read a product" },
                new Permission { Id = 8, Name = "Write-Product", Description = "Permission to create/Edit a product" },
                new Permission { Id = 9, Name = "Delete-Product", Description = "Permission to Delete a product" },
                // Brand Permissions
                new Permission { Id = 10, Name = "Read-Brand", Description = "Permission to Read a brand" },
                new Permission { Id = 11, Name = "Write-Brand", Description = "Permission to create/Edit a brand" },
                new Permission { Id = 12, Name = "Delete-Brand", Description = "Permission to Delete a brand" },
                // Category Permissions
                new Permission { Id = 13, Name = "Read-Category", Description = "Permission to Read a category" },
                new Permission { Id = 14, Name = "Write-Category", Description = "Permission to create/Edit a category" },
                new Permission { Id = 15, Name = "Delete-Category", Description = "Permission to Delete a category" }
            };
            modelBuilder.Entity<Permission>().HasData(permissionsToSeed);

            // Seed Roles
            var adminRoleId = 1;
            var cutomerRoleID = 2;
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = adminRoleId, Name = "Admin", Description = "Administrator with full permissions" },
                new Role { Id = cutomerRoleID, Name = "Customer", Description = "Customer with limited permissions" }
            );

            // link Permissions to admin Role
            int rolePermissionId = 1;
            var adminRolePermissions = permissionsToSeed.Select(permission => new RolePermission
            {
                Id = rolePermissionId++,
                RoleId = adminRoleId,
                PermissionId = permission.Id
            });
            modelBuilder.Entity<RolePermission>().HasData(adminRolePermissions);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    RoleId = adminRoleId,
                    Name = "Admin User",
                    Email = "admin@example.com",
                    UserName = "admin",
                    Address = "123 Admin Street",
                    BirthDate = new DateOnly(1990, 1, 1),
                    AvatarUrl = null,
                    Password = "AQAAAAIAAYagAAAAEHLveeuTI0BCPkw8snPk0ZKIOPTgBgpl88rDkbkQFgk7k9alASCIeJJBRiMigrK8sA=="
                }
            );

            modelBuilder.Entity<Domain.Entities.User>()
                .HasOne(b => b.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(b => b.RoleId)
                 ;

            modelBuilder.Entity<RolePermission>()
                .HasOne(b => b.Role)
                .WithMany(t => t.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                ;

            modelBuilder.Entity<RolePermission>()
                .HasOne(b => b.Permission)
                .WithMany(t => t.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                ;

            modelBuilder.Entity<Invoice>()
                .HasOne(b => b.User)
                .WithMany(t => t.Invoices)
                .HasForeignKey(e => e.UserId)
                ;

            modelBuilder.Entity<Invoice>()
                .HasOne(b => b.Product)
                .WithMany(t => t.Invoices)
                .HasForeignKey(e => e.ProductId)
               ;


            modelBuilder.Entity<Product>()
                .HasOne(b => b.Brand)
                .WithMany(t => t.Products)
                .HasForeignKey(b => b.BrandId)
                ;

            modelBuilder.Entity<Product>()
                .HasOne(b => b.Category)
                .WithMany(t => t.Products)
                .HasForeignKey(b => b.CategoryId)
                ;

            modelBuilder.Entity<Facility>()
                .HasOne(b => b.Product)
                .WithMany(t => t.Facilities)
                .HasForeignKey(b => b.ProductId)
                ;
        }
    }
}

