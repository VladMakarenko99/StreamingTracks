import {Component, Input, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {ActivatedRoute, Router, RouterLink, RouterOutlet} from '@angular/router';
import { environment } from '../../../environments/environment';
import { Soundtrack } from '../../models/soundtrack';
import { NgIf, NgClass, NgOptimizedImage } from '@angular/common';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import {AudioPlayerComponent} from "../audio-player/audio-player.component";
import {UtilitiesService} from "../../shared/utilities.service";
import {Subscription} from "rxjs";
import {MatIcon} from "@angular/material/icon";


@Component({
  selector: 'app-soundtrack',
  standalone: true,
  imports: [NgIf, NgClass, NgOptimizedImage, MatProgressSpinner, MatProgressSpinnerModule, AudioPlayerComponent, RouterLink, MatIcon, RouterOutlet],
  templateUrl: './soundtrack.component.html',
  styleUrls: ['./soundtrack.component.css']
})
export class SoundtrackComponent implements OnInit {
  soundtrack: Soundtrack | null = null;
  isLoading: boolean = false;
  albumCoverUrl: string = `${environment.apiUrl}/api/Soundtrack/image/`;
  private routeSub: Subscription | null = null;

  constructor(private http: HttpClient,
              private route: ActivatedRoute,
              public utilities: UtilitiesService,
              private router: Router) {}

  ngOnInit(): void {
    this.routeSub = this.route.paramMap.subscribe((paramMap) => {
      console.log("changed")
      const slug = paramMap.get('slug');
      if (slug) {
        this.fetchSoundtrack(slug);
      }
    });
  }

  fetchSoundtrack(slug: string): void {
    console.log("FETCHING: ")

    this.isLoading = true;

    this.http.get<Soundtrack>(`${environment.apiUrl}/api/Soundtrack/${slug}`).subscribe({
      next: (result: any) => {
        this.isLoading = false;
        if (result.isSuccess) {
          this.soundtrack = result.body;
          console.log(this.soundtrack);
        } else {
          console.error("Error: Unable to fetch soundtrack details.");
        }
      },
      error: (error) => {
        console.error("Error fetching soundtrack:", error);
        this.isLoading = false;
      }
    });
  }

  navigateToTrack(trackId: string): void {
    if (trackId) {
      console.log("navigating to track")
      this.router.navigate(['/soundtrack', trackId], { queryParamsHandling: 'preserve' });
    }
  }

  ngOnDestroy(): void {
    if (this.routeSub) {
      this.routeSub.unsubscribe();
    }
  }

}
