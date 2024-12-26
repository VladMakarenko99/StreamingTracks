import {Component, HostListener} from '@angular/core';
import { Soundtrack } from '../../models/soundtrack';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import {NgClass, NgFor, NgIf, NgOptimizedImage} from '@angular/common';
import { UploadTrackComponent } from "../upload-track/upload-track.component";
import { AuthService } from '../../services/auth.service';
import { DeleteButtonComponent } from "../delete-button/delete-button.component";
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { LoggedInUser } from '../../models/logged-user';
import {AudioPlayerComponent} from "../audio-player/audio-player.component";
import {MatProgressSpinner, MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {RouterLink} from "@angular/router";
import {UtilitiesService} from "../../shared/utilities.service";

@Component({
  selector: 'app-soundtrack-list',
  standalone: true,
  imports: [NgFor, NgIf, UploadTrackComponent, DeleteButtonComponent, MatIconModule, AudioPlayerComponent, MatProgressSpinner, MatProgressSpinnerModule, NgOptimizedImage, NgClass, RouterLink],
  templateUrl: './soundtrack-list.component.html',
  styleUrl: './soundtrack-list.component.css'
})
export class SoundtrackListComponent {
  soundTackList: Soundtrack[] = [];
  isLoggedIn: boolean = false;
  isAdmin: boolean = false;
  loggedUser: LoggedInUser | null = null;
  isLoading: boolean = false;
  albumCoverUrl: string = `${environment.apiUrl}/api/Soundtrack/image/`;

  constructor(private http: HttpClient, private authService: AuthService, public utilityService: UtilitiesService) {
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
          this.isLoading = false;
          this.soundTackList = result.body.map((track: Soundtrack) => ({ ...track, showPlayer: false }));
        },
        error: (error) => {
          console.error("Error fetching soundtracks:", error);
          this.isLoading = false;
        }
      });
  }


  togglePlayer(track: Soundtrack): void {
    track.showPlayer = !track.showPlayer;
    console.log(track.showPlayer);
  }
}
