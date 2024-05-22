import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddBlogPost } from '../models/add-blog-post.model';
import { BlogPost } from '../models/blog-post.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Injectable({
  providedIn: 'root'
})
export class BlogPostService {

  constructor(private http: HttpClient) { }

  createBlogPost(data: AddBlogPost) : Observable<BlogPost> {
     return this.http.post<BlogPost>(`${environment.apiBasedUrl}/api/blogposts`, data);
  } 

  getAllBlogPosts() : Observable<BlogPost[]>{
    // let params = new HttpParams();
    // if(TitleText) {
    //   params = params.set('TitleText',TitleText)
    // }
    return this.http.get<BlogPost[]>(`${environment.apiBasedUrl}/api/Blogposts`);
  }
  // getAllBlogPostByTitle(TitleText?: string) : Observable<BlogPost[]> {
  //   let params = new HttpParams();
  //   if(TitleText) {
  //     params = params.set('TitleText', TitleText) 
  //   }
  //   return this.http.get<BlogPost[]>(`${environment.apiBasedUrl}/api/BlogPosts/title`,{
  //     params: params });

  // }
  getAllBlogPostByTitle(titleText?: string): Observable<BlogPost[]> {
    let params = new HttpParams();
    if (titleText) {
      params = params.set('title', titleText); // Ensure 'title' matches backend parameter name
    }
    return this.http.get<BlogPost[]>(`${environment.apiBasedUrl}/api/BlogPosts/title/${titleText}`, {
      params: params
    });
  }


  getBlogPostById(id: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${environment.apiBasedUrl}/api/Blogposts/${id}`);
  }

  // getBlogPostByUrlHandle(urlHandle: string): Observable<BlogPost> {
  //   return this.http.get<BlogPost>(`${environment.apiBasedUrl}/api/blogposts/${urlHandle}`);
  // }
  getBlogPostByUrlHandle(urlHandle: string): Observable<BlogPost> {
    return this.http.get<BlogPost>(`${environment.apiBasedUrl}/api/BlogPosts/url/${urlHandle}`);
  }
  

  updateBlogPost(id: string, updateBlogPost: UpdateBlogPost): Observable<BlogPost> {
    return this.http.put<BlogPost>(`${environment.apiBasedUrl}/api/blogposts/${id}`,updateBlogPost)
  }
  deleteBlogPost(id:string): Observable<HttpResponse<any>> {
    return this.http.delete<BlogPost>(`${environment.apiBasedUrl}/api/blogposts/${id}`, {observe: 'response'});
  }
}
