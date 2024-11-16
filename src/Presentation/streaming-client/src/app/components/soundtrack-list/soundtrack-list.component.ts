import { Component } from '@angular/core';
import { Soundtrack } from '../../models/soundtrack';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { NgFor, NgIf } from '@angular/common';
import { UploadTrackComponent } from "../upload-track/upload-track.component";
import { AuthService } from '../../services/auth.service';
import { DeleteButtonComponent } from "../delete-button/delete-button.component";
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { LoggedInUser } from '../../models/logged-user';
import {AudioPlayerComponent} from "../audio-player/audio-player.component";
import {MatProgressSpinner, MatProgressSpinnerModule} from "@angular/material/progress-spinner";

@Component({
  selector: 'app-soundtrack-list',
  standalone: true,
  imports: [NgFor, NgIf, UploadTrackComponent, DeleteButtonComponent, MatIconModule, AudioPlayerComponent, MatProgressSpinner, MatProgressSpinnerModule],
  templateUrl: './soundtrack-list.component.html',
  styleUrl: './soundtrack-list.component.css'
})
export class SoundtrackListComponent {
  soundTackList: Soundtrack[] = [];
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  loggedUser: LoggedInUser | null = null;
  isLoading: boolean = false;
  albumCoverUrl: string = '';
  defaultImageUrl: string = 'album.png';

  constructor(private http: HttpClient, private authService: AuthService) {
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this.isLoggedIn = isLoggedIn;
      this.loggedUser = authService.getUser();
    });
  }

  ngOnInit(): void {
    this.fetchSoundTracks();
    this.isAdmin = this.loggedUser?.role == 'Admin';
  }

  fetchSoundTracks(): void {
    this.isLoading = true;

    this.http.get<Soundtrack[]>(`${environment.apiUrl}/api/Soundtrack`)
      .subscribe({
        next: (result: any) => {
          this.soundTackList = result.body;
          this.isLoading = false;
        },
        error: (error) => {
          console.error("Error fetching soundtracks:", error);
          this.isLoading = false;
        }
      });
  }

  convertSecondsToTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
  }

  onImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = this.defaultImageUrl; // Replace with the default image
  }
}
