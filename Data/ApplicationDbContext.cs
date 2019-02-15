using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdsServer.Models;

namespace IdsServer.Data
{
    public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser,ApplicationRole,int,ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
             modelBuilder.Entity<ApplicationRoleClaim>(builder =>
        {
            builder.HasOne(roleClaim => roleClaim.ApplicationRole).WithMany(role => role.RoleClaims).HasForeignKey(roleClaim => roleClaim.RoleId);
            // builder.ToTable("RoleClaim");
        });
        }
    }
}
