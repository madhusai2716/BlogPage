import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { RegisterRequest } from '../../models/register-request.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model: RegisterRequest;
  errorMessage: string | null = null;
  invalidEmailFormat: boolean = false;
  invalidPasswordFormat: boolean = false;
  showLoader1: boolean = false;

  constructor(private http: HttpClient, private router: Router) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit() {
    this.showLoader1 = true;
    this.http.post<any>('https://localhost:7022/api/AuthContoller/register', this.model)
      .subscribe(
        response => {
          // Registration successful, redirect to login page
          console.log(response);
          this.router.navigate(['/login']);
          this.showLoader1 =false;
        },
        error => {
          // Registration failed, display error message
          if (error.status === 400 && error.error === "Invalid email address format.") {
            this.invalidEmailFormat = true;
            this.showLoader1=false;
          }
          else if (error.status === 400 && error.error === "Password must be atleast 6 characters and atleast one special character.") {
            this.invalidPasswordFormat = true;
            this.showLoader1=false;
          }
          else {
          this.errorMessage = error.error.message;
          this.showLoader1=false;
          }
        }
      );
  }
}
