// CategoryRepository.cs
using BloggingCode.API.Data;
using BloggingCode.API.Models.Doamin;
using BloggingCode.API.Repo.Interface;
using Microsoft.EntityFrameworkCore; // Add this namespace if not already added
using System.Linq; // Add this namespace if not already added

namespace BloggingCode.API.Repo.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null)
        {
            //query the databse 
            //categories variable is a Iqueryable
            var categories = dbContext.Categories.AsQueryable();
            //Filtering using the query
            if(string.IsNullOrWhiteSpace(query) == false)
            {
                //entity farmework will handle the case senstivity while searching for required category
                categories = categories.Where(x => x.Name.Contains(query));
            }
            //return await dbContext.Categories.ToListAsync();
            return await categories.ToListAsync();

        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory= await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            
            if(existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;

            }
            return null;
                        
        }

        //delete
        public async Task<Category> DeleteAsync(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (existingCategory is null)
                {
                return null;
                }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }
    }
}
