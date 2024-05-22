namespace BloggingCode.API.Models.DTO
{
    public class CategoryDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        public Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string? Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? UrlHandle { get; set; }
    }
}
