import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { BlogPostService } from '../services/blog-post.service';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css']
})
export class BlogpostListComponent implements OnInit{


  blogPosts$?: Observable<BlogPost[]>;


  constructor(private blogPostService: BlogPostService) {

  }

  ngOnInit(): void {
    //get all blog post from api
    this.blogPosts$ = this.blogPostService.getAllBlogPosts();
  }

 
  onSearch(TitleText: string) {
    // Call the service method to get blog posts by title
    if (TitleText.trim()) {
      this.blogPosts$ = this.blogPostService.getAllBlogPostByTitle(TitleText);
    } else {
      // If search text is empty, fetch all blog posts
      this.blogPostService.getAllBlogPosts();
    }
 

  
  
}
}
