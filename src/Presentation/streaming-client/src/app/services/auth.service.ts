import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.checkLoginStatus());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor() {}

  private checkLoginStatus(): boolean {
    return !!localStorage.getItem('access_token');
  }

  logIn(token: string): void {
    localStorage.setItem('access_token', JSON.stringify(token));
    this.isLoggedInSubject.next(true); 
  }

}
