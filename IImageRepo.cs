using BloggingCode.API.Models.Domain;
using System.Net;

namespace BloggingCode.API.Repo.Interface
{
    public interface IImageRepo
    {
       Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAll();
    }
}
