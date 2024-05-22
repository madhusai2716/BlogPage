import { query } from '@angular/animations';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient,
    private cookieService: CookieService) {}

  getAllCategories(query?: string): Observable<Category[]>{
    let params = new HttpParams();
    if(query) {
      params = params.set('query', query)
    }
    return this.http.get<Category[]>(`${environment.apiBasedUrl}/api/Categories`, {
      params: params });

  }

  getCatgeoryById(id: string): Observable<Category> {
      return this.http.get<Category>(`${environment.apiBasedUrl}/api/Categories/${id}`);
  }

  addCategory(model: AddCategoryRequest): Observable<void> {
    return this.http.post<void>(`${environment.apiBasedUrl}/api/Categories`, model ,
    );
  }
 
  updateCategory(id: string,updateCategoryRequest: UpdateCategoryRequest): Observable<Category> {
   return  this.http.put<Category>(`https://localhost:7022/api/Categories/${id}`, updateCategoryRequest,{
    headers: {
      'Authorization': this.cookieService.get('Authorization')
    }
   });

  }
  deleteCategory(id: string) : Observable<Category> {
    return this.http.delete<Category>(`${environment.apiBasedUrl}/api/Categories/${id}`,);
  }
}
