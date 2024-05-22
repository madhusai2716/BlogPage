using BloggingCode.API.Models.Domain;
using BloggingCode.API.Repo.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingCode.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepo imageRepo;
        public ImagesController(IImageRepo imageRepo) {
            this.imageRepo = imageRepo;
        
        }
        //Get: {apibasedUrl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            //call image repo to get all images
            var images = await imageRepo.GetAll();
            return Ok(images);
        }
        //POST: {abibasedUrl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile file,[FromForm]string fileName,[FromForm] string title)
        {

            ValidateFileUpload(file);

            if(ModelState.IsValid)
            {
                //File Upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

               blogImage =  await imageRepo.Upload(file, blogImage);
                return Ok(blogImage);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if(allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            //file should not  be more than 10mb
            if(file.Length > 52428800)
            {
                ModelState.AddModelError("file", "File cannot be more than 10Mb");
            }
        }
    }
}
