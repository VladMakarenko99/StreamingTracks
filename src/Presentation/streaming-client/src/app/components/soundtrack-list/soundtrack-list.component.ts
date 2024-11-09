import { Component } from '@angular/core';
import { Soundtrack } from '../../models/soundtrack';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { NgFor, NgIf } from '@angular/common';
import { UploadTrackComponent } from "../upload-track/upload-track.component";
import { AuthService } from '../../services/auth.service';
import { DeleteButtonComponent } from "../delete-button/delete-button.component";

@Component({
  selector: 'app-soundtrack-list',
  standalone: true,
  imports: [NgFor, NgIf, UploadTrackComponent, DeleteButtonComponent],
  templateUrl: './soundtrack-list.component.html',
  styleUrl: './soundtrack-list.component.css'
})
export class SoundtrackListComponent {
  soundTackList: Soundtrack[] = [];
  isAdmin: boolean = false;

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    this.fetchSoundTracks();
    this.isAdmin = this.authService.getUser()?.role == 'Admin'
  }

  fetchSoundTracks(): void {
    this.http.get<Soundtrack[]>(`${environment.apiUrl}/api/Soundtrack`)
      .subscribe((result: any) => {
        this.soundTackList = result.body
      });
  }

  convertSecondsToTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
  }
}
