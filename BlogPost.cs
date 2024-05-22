using System.ComponentModel.DataAnnotations;

namespace BloggingCode.API.Models.Doamin
{
    public class BlogPost
    {
        [Key] 
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; }    
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }  
        public bool IsVisibble { get; set; }

        public ICollection<Category> Categories { get; set; }


    }
    
}
