using System.ComponentModel.DataAnnotations;

namespace BloggingCode.API.Models.DTO
{
    public class CreateCategoryRequestDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? UrlHandle { get; set; }
    }
}
