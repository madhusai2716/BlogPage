using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloggingCode.API.Repo.Interface;
using BloggingCode.API.Models.Doamin;
using Microsoft.AspNetCore.Authorization;
using BloggingCode.API.Models.DTO;
using BloggingCode.API.Repo.Implementation;

namespace BloggingCode.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        //dependancy injection
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
    
        }

        
        [HttpGet]
        
        public async Task<IActionResult> GetBlogPosts()
         {
             var blogPosts = await _blogPostRepository.GetAllAsync();
             //convert domain model to dto
             var response = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    IsVisibble = blogPost.IsVisibble,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    Title = blogPost.Title,
                    UrlHandle = blogPost.UrlHandle,
                    Categories = blogPost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                });

                // Map categories from domain model to DTOs and assign them to BlogPostDto
                /*if(blogPost.Categories != null && blogPost.Categories.Any())
                {
                   blogPost.Categories = (ICollection<Category>)blogPost.Categories.Select(x => new CategoryDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       UrlHandle = x.UrlHandle
                   }).ToList();
                }

                response.Add(blogPostDto);
            }*/
            }
             return Ok(response);
         }


        // GET: api/BlogPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        //Get by url 
        [HttpGet("url/{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle(string urlHandle)
        {
            //Get blogpost details from repo
            var blogPost =await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            return Ok(blogPost);

        }

        //get by title
        // GET: api/BlogPosts/title/{title}
        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetBlogPostsByTitle(string title)
        {
            var blogPosts = await _blogPostRepository.GetByTitleAsync(title);

            if (blogPosts == null || !blogPosts.Any())
            {
                return NotFound();
            }

            var response = blogPosts.Select(blogPost => new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisibble = blogPost.IsVisibble,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            }).ToList();

            return Ok(response);
        }


        // POST: api/BlogPosts
        [HttpPost]
        
        public async Task<IActionResult> PostBlogPost(CreateBlogPostRequestDto request)
        {
            //convert dto to domain
            var blogPost = new BlogPost
            {
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisibble = request.IsVisibble,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if(existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await _blogPostRepository.CreateAsync(blogPost);

            //convert Domain model to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisibble = blogPost.IsVisibble,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()

            };
            return Ok(response);
        }
        
        //PUT
        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdatedBlogPostId(Guid id, UpdateBlogPostRequestDto request)
        {
            //convert dto to domain 
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisibble = request.IsVisibble,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            //foreach loop to all the categories
            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if(existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            var updatedBLogPost = await _blogPostRepository.UpdateAsync(blogPost);
            if(updatedBLogPost == null)
            {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisibble = blogPost.IsVisibble,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            };
            return Ok(response);
        }

        
        // DELETE: api/BlogPosts/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(id);
            if(deletedBlogPost == null)
            {
                //BlogPost was succesfully deletd , return no content
                return Ok("Deleted succesfully ");
            }
            // If the deletedBlogPost is not null, it means that something went wrong
            // and the blog post was not deleted. In this case, you can return NotFound.
            else
            {
                return NotFound();
            }
        
            
        }
    }
}
