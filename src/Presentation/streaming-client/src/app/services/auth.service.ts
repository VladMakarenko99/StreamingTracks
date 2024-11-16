import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LoggedInUser } from '../models/logged-user';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.checkLoginStatus());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor() { }

  private checkLoginStatus(): boolean {
    return !!localStorage.getItem('access_token');
  }

  logIn(token: string): void {
    localStorage.setItem('access_token', token);
    this.isLoggedInSubject.next(true);
  }

  logOut(): void {
    localStorage.removeItem('access_token');
    this.isLoggedInSubject.next(false);
  }

  getUser(): LoggedInUser | null {
    const token = localStorage.getItem('access_token');

    if (!token) {
      return null;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      const user: LoggedInUser = {
        firstName: decodedToken.FirstName,
        lastName: decodedToken.LastName,
        email: decodedToken.Email,
        role: decodedToken.Role
      };
      return user;
    } catch (error) {
      return null;
    }
  }
}
