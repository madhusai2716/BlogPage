import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';
import { UpdateBlogPost } from '../models/update-blog-post.model';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit , OnDestroy{
  id: string | null = null;
  model?: BlogPost;
  categories$? : Observable<Category[]>;
  selectedCategories?: string[];
  isImageSelectorVisible: boolean = false;
  
  routeSubscription?: Subscription;
  updateBlogPostSubscription?: Subscription;
  getBlogSubscription?: Subscription;
  deleteBlogSubscription?: Subscription;
  imageSelectSubscription? :Subscription;


  constructor(private route: ActivatedRoute,
    private blogPostService: BlogPostService,
    private categoryService: CategoryService,
    private router: Router,
    private imageService: ImageService) {

  }
  
  ngOnInit(): void {
    this.categories$ =this.categoryService.getAllCategories();
    this.routeSubscription=this.route.paramMap.subscribe({
      next: (params) => {
        this.id=params.get('id');
        //Get BLOGPOSt from API
        if (this.id) {
        this.getBlogSubscription = this.blogPostService.getBlogPostById(this.id)
        .subscribe({
          next: (response) => {
            this.model = response;
            this.selectedCategories = response.categories?.map(x => x.id) ?? [];
          }
        });
        }
        //this is for select function to select the required image in ui
        this.imageSelectSubscription = this.imageService.onSelectImage()
        .subscribe({
          next: (response) =>
          {
            if(this.model) {
              this.model.featuredImageUrl = response.url;
              this.isImageSelectorVisible = false;
            }
          }
        })
      }
    });
  }
  onFormSubmit(): void{

    //convert this model to a request object
    if(this.model && this.id )
    {
      var updateBlogPost: UpdateBlogPost = {
        author: this.model.author,
        content: this.model.content,
        shortDescription: this.model.shortDescription,
        featuredImageUrl: this.model.featuredImageUrl,
        isVisibble: this.model.isVisibble,
        publishedDate: this.model.publishedDate,
        title: this.model.title,
        urlHandle: this.model.urlHandle,
        categories: this.selectedCategories ?? [] 

      };

      this.updateBlogPostSubscription = this.blogPostService.updateBlogPost(this.id, updateBlogPost).subscribe({
        next: (response) => {
          alert('Blog has been updated successfully');
         this.router.navigateByUrl('/admin/blogposts');
        }
      });
    }


  }

  onDelete(): void {
    if(this.id) 
    {
      //call service and delete blogpost
      this.deleteBlogSubscription = this.blogPostService.deleteBlogPost(this.id)
      .subscribe({
        next: (response) => {
          alert("Deleted successfully");
          this.router.navigateByUrl('/admin/blogposts');
          // this.router.navigateByUrl('/admin/blogposts').then(
          //   () => console.log('Navigation succeeded'),
          //   (error) => console.error('Navigation failed:', error)
          // );
          
        }
      });
    }
  }
  // isDeleted: boolean = false;

  // onDelete(): void {
  //   if (this.id) {
  //     this.deleteBlogSubscription = this.blogPostService.deleteBlogPost(this.id)
  //       .subscribe({
  //         next: (response) => {
  //           if (response.status === 204) {
  //             this.isDeleted = true; // Set deletion status to true
  //             // Optionally, you can provide feedback to the user here
  //             this.router.navigateByUrl('/admin/blogposts');
  //           } else if (response.status === 200) {
  //             // Handle successful deletion with response body, if needed
  //             console.log('Blog post deleted, response body:', response.body);
  //           } else {
  //             // Handle other status codes, if needed
  //             console.warn('Unexpected status code:', response.status);
  //           }
  //         },
  //         error: (error) => {
  //           console.error('Error occurred during deletion:', error);
  //         }
  //       });
  //   }
  // }



  openImageSelector(): void {
    this.isImageSelectorVisible = true;

  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }
  

  ngOnDestroy(): void {
    this.routeSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.getBlogSubscription?.unsubscribe();
    this.deleteBlogSubscription?.unsubscribe();
    this.imageSelectSubscription?.unsubscribe();
  }

}
