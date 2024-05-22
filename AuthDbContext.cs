using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BloggingCode.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //create reader and writer role for the blog page
            var readerRoleId = "75c34a32-34e9-4a27-9acb-bb185dec22d4";
            var writerRoleId = "b92ade6a-d3bb-494b-b997-127252d75285";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //seed the roles
            builder.Entity<IdentityRole>().HasData(roles);
            //create an Admin USer
            var adminUserId = "c833de75-4db1-4352-adea-5f271dbc0b69";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@blogpage.com",
                Email = "admin@blogpage.com",
                NormalizedEmail = "admin@blogpage.com",
                NormalizedUserName = "admin@blogpage.com"
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin123");
            builder.Entity<IdentityUser>().HasData(admin);

            //Give roles to admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId

                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);


        }
    }
}
