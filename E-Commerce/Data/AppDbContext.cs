using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime;
using System.Security.Policy;

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
            base.OnModelCreating(modelBuilder);
        }
    }
}

