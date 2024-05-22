using BloggingCode.API.Models.Doamin;

namespace BloggingCode.API.Repo.Interface
{
    public interface IBlogPostRepository
    {
        
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<IEnumerable<BlogPost>> GetByTitleAsync(string title);

        Task<BlogPost> GetByIdAsync(Guid id);
        Task<BlogPost> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> UpdateAsync(BlogPost blogPost);
        
        Task<BlogPost?> DeleteAsync(Guid id);

    }
}
