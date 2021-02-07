using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region "Seed Data"

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PasswordHash = hasher.HashPassword(null, "Admin@123")
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
            );

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id=1,
                    Name = "Apparels",
                    Description = "Apparels"
                }
            );

            #endregion
        }
    }
}
