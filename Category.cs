using Microsoft.Build.Framework;


namespace BloggingCode.API.Models.Doamin
{
    public class Category
    {
        [System.ComponentModel.DataAnnotations.Required]
        public Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? UrlHandle { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
