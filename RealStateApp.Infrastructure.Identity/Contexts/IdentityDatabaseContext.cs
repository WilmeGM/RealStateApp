using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.Contexts
{
    public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable("Users"));
            modelBuilder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
