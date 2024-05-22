using BloggingCode.API.Models.Doamin;

namespace BloggingCode.API.Repo.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(string? query = null);
        Task<Category> GetByIdAsync(Guid id);

        Task<Category?> UpdateAsync(Category category);

        Task<Category> DeleteAsync(Guid id);
    }
}
