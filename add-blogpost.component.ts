import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { AddBlogPost } from '../models/add-blog-post.model';
import { BlogPostService } from '../services/blog-post.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent implements OnInit , OnDestroy{

  model: AddBlogPost;
  // 
  categories$?: Observable<Category[]>;
  isImageSelectorVisible: boolean = false;
  imageSelectorSubscription?: Subscription;

  constructor(private blogPostService: BlogPostService,
    private router : Router,
    private categoryService: CategoryService,
    private imageService: ImageService) {
    this.model = {
      title: '',
      shortDescription: '',
      urlHandle: '',
      content: '',
      featuredImageUrl: '',
      author: '',
      isVisibble: true,
      publishedDate: new Date(),
      categories: []
    } 
  }
 
  ngOnInit(): void {
   this.categories$ = this.categoryService.getAllCategories(); 

  this.imageSelectorSubscription = this.imageService.onSelectImage()
  .subscribe({
    next: (selectedImage) =>{
    this.model.featuredImageUrl = selectedImage.url;
    this.closeImageSelector();
    
     
    }
  })


  }

  onFormSubmit(): void{
     console.log(this.model);
         this.blogPostService.createBlogPost(this.model)
         .subscribe ({
          next: (response) => {

            alert('Blog Added successfully');
            this.router.navigateByUrl('/admin/blogposts');
            
          }

         });
         
  }

  openImageSelector(): void {
    this.isImageSelectorVisible = true;

  }

  closeImageSelector(): void {
    this.isImageSelectorVisible = false;
  }
  ngOnDestroy(): void {
    this.imageSelectorSubscription?.unsubscribe();
  }

}
