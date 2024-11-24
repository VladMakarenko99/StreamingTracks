import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { SignInModel } from '../../models/sign-in.model';
import { NgIf } from '@angular/common';
import { Route, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-sign-in',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  constructor(private http: HttpClient, private router: Router, private authService: AuthService) { }
  signInData: SignInModel = {
    email: '',
    password: ''
  };


  errorMessage: string | null = null;

  onSubmit() {
    console.log(this.signInData);
    this.http.post<string>(`${environment.apiUrl}/api/Auth/sign-in`, this.signInData,  { responseType: 'text' as 'json' })
      .subscribe({
        next: (response) => {
          this.authService.logIn(response);
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          if (error.status === 401) {
            this.errorMessage = error.error || "Login failed. Please check your credentials.";
          } else {
            this.errorMessage = "An unexpected error occurred. Please try again later.";
          }
        }
      });
  }
}
