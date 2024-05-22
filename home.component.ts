import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../Auth/models/user.model';
import { AuthService } from '../../Auth/services/auth.service';
import { BlogPost } from '../../blog-post/models/blog-post.model';
import { BlogPostService } from '../../blog-post/services/blog-post.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{


  user?: User; 
  isLoggedIn: boolean = false;
  

  //ngOnINIT will return an observable and ? is used for undefined
  blog$?: Observable<BlogPost[]>;
  constructor(private blogPostService: BlogPostService,
    private authService: AuthService) {


  }
  ngOnInit(): void {
    
    this.blog$ = this.blogPostService.getAllBlogPosts();
    
    this.isLoggedIn = this.authService.IsLoggedIn() !== null;
   
    // i also need to give logout so that it should the user has been logged out and it should not show the onSearchHome
    //also ask chatgpt for admin while he is using dropdown the search should go down
    

  }

  trackByFn(index: number, item: BlogPost) {
    return item.id; // Assuming 'id' is the unique identifier field in your BlogPost model
  }
  onSearchHome(TitleText: string) {

    this.blog$= this.blogPostService.getAllBlogPostByTitle(TitleText);
    
    
  }
}
