import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { SignInModel } from '../../models/sign-in.model';
import { NgIf } from '@angular/common';
import { Route, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';


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

  onSubmit() {
    console.log(this.signInData);
    this.http.post<string>('http://localhost:5064/api/Auth/login', this.signInData,  { responseType: 'text' as 'json' })
      .subscribe(response => {
        this.authService.logIn(response);
        this.router.navigateByUrl('/');
      });
  }

}
