import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';
import { CategoryService } from '../services/category.service';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit,OnDestroy{


  id: string | null = null;
  paramsSubscription?: Subscription;
  editCategorySubscription?: Subscription;
  category?: Category;

  constructor(private route: ActivatedRoute,
    private categoryService:CategoryService,
    private router: Router ) {

  }

  ngOnInit(): void {
   this.paramsSubscription =  this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');


        if(this.id) {
          // get the data from the api for this category id
          this.categoryService.getCatgeoryById(this.id)
          .subscribe({
            next: (response) => {
                this.category = response;
            }
          });


        }
      }
    });
  }


  onFormSubmit(): void{
    const updateCategoryRequest: UpdateCategoryRequest = {
      name: this.category?.name ?? '',
      urlHandle: this.category?.urlHandle ?? ''
    };
    //pass this object to service
    if(this.id) {
    this.editCategorySubscription = this.categoryService.updateCategory(this.id,updateCategoryRequest)
    .subscribe({
      next: (response) => {
        alert('Category has been updated successfully');
        console.log('Submitted');
         this.router.navigateByUrl('admin/category');
      }
    });
    }
  }


  onDelete(): void{
    if (this.id) {
    this.categoryService.deleteCategory(this.id)
    .subscribe({
      next: (response) => {
        alert('Category Deleted');
        this.router.navigateByUrl('admin/category');
        console.log("Deleted");
      }
    })
    }
  }


  ngOnDestroy(): void {
    this.paramsSubscription?.unsubscribe();
    this.editCategorySubscription?.unsubscribe();
  }

}
