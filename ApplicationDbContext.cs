using BloggingCode.API.Models.Doamin;
using BloggingCode.API.Models.Domain;

using Microsoft.EntityFrameworkCore;


namespace BloggingCode.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }

        
    }
}
