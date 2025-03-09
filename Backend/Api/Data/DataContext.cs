using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Api.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            builder.Entity<AppUser>().ToTable("Users");
            builder.Entity<AppRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("UserTokens");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        }
    }
}
