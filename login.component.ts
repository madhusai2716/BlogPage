import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { LoginRequest } from '../models/login-request.model';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

 
  model: LoginRequest;
  errorMessage: string='';
  showLoader: boolean = false;

  constructor(private authService: AuthService,
    private cookieService: CookieService,
    private router: Router) {
    this.model = {
      email: '',
      password: ''
    };
  }

  onFormSubmit(): void{
    this.showLoader = true;
   this.authService.login(this.model)
   .subscribe({
    next: (response) => {
      
      //for this below code we need to instal npm install ngx cookie service package
      this.cookieService.set('Authorization','Bearer ${response.token}',
      undefined,'/',undefined,true,'Strict');
      //we wnat to set the user in the local storage

      this.authService.setUser({
        email: response.email,
        roles: response.roles
      });
      //Redirect back to home pafge after login

      this.router.navigateByUrl('/');
      alert('Login successfull');
      this.showLoader = false;

    },
   
    error: (error) => {
      this.errorMessage = error.error;
      this.showLoader = false; // Use the error message received from the backend
    }
   });
  }

  

  

}
