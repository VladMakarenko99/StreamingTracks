import { Component } from '@angular/core';
import { Soundtrack } from '../../models/soundtrack';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { NgFor, NgIf } from '@angular/common';
import { UploadTrackComponent } from "../upload-track/upload-track.component";

@Component({
  selector: 'app-soundtrack-list',
  standalone: true,
  imports: [NgFor, NgIf, UploadTrackComponent],
  templateUrl: './soundtrack-list.component.html',
  styleUrl: './soundtrack-list.component.css'
})
export class SoundtrackListComponent {
  soundTackList: Soundtrack[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchSoundTracks();
  }

  fetchSoundTracks(): void {
    this.http.get<Soundtrack[]>(`${environment.apiUrl}/api/Soundtrack`)
      .subscribe((result: any) => {
        this.soundTackList = result.body
        console.log(this.soundTackList)
      });
  }




  convertSecondsToTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
  }
}
