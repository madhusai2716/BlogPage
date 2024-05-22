using BloggingCode.API.Data;
using BloggingCode.API.Models.Doamin;
using BloggingCode.API.Repo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BloggingCode.API.Repo.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        //dependency injection
        private readonly ApplicationDbContext _dbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            _dbContext.BlogPosts.Add(blogPost);
            await _dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
         return await _dbContext.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<IEnumerable<BlogPost>> GetByTitleAsync(string title)
        {
            return await _dbContext.BlogPosts
                .Where(x => x.Title.Contains(title))
                .Include(x => x.Categories)
                .ToListAsync();
        }


        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            return await _dbContext.BlogPosts.FindAsync(id);
        }

        public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
        {
            //return await _dbContext.BlogPosts.FindAsync(urlHandle); 
            //Include(x => x.Categories).FirstOrDefault(x => x.UrlHandle == urlHandle);
            return await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            //searching for the blog
            var existingBlogPost = await _dbContext.BlogPosts.Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            //checking if it null
            if(existingBlogPost == null)
            {
                return null;
            }
            //UPdate BlogPost
            _dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);
            //Update Categories
            existingBlogPost.Categories = blogPost.Categories;
            await _dbContext.SaveChangesAsync();
            return blogPost;
        }


        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
            if(existingBlogPost != null)
            {
                _dbContext.BlogPosts.Remove(existingBlogPost);
                await _dbContext.SaveChangesAsync();
            }
            return null;

        }
    }
}
