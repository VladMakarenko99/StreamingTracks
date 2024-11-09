import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { LoggedInUser } from '../../models/logged-user';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, NgIf], 
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  isLoggedIn: boolean = false;
  loggedUser: LoggedInUser | null = null;

  constructor(private authService: AuthService) {
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      this.loggedUser = authService.getUser()
    });
  }
}
