import {environment} from "../../../environments/environment";
import {
  Component,
  Input,
  OnDestroy,
  SimpleChanges,
  OnChanges,
  ViewChild,
  ElementRef, Output, EventEmitter
} from "@angular/core";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [],
  templateUrl: './audio-player.component.html',
  styleUrl: './audio-player.component.css'
})
export class AudioPlayerComponent implements OnChanges, OnDestroy {
  @Input() id: string = '';
  @Input() audioUrl: string = '';
  @Input() albumCoverUrl: string = '';
  @Input() title: string = 'Default Title';
  @Input() prevTrackId: string = '';
  @Input() nextTrackId: string = '';
  @ViewChild('audioElement', {static: false}) audioElementRef!: ElementRef<HTMLAudioElement>;
  private viewInitialized = false;

  constructor(private router: Router, private http: HttpClient) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['id'] || changes['albumCoverName'] || changes['title']) {
      this.updatePlayer();
      this.playAudio();
      this.increaseListenings();
    }
  }

  ngAfterViewInit(): void {
    this.viewInitialized = true;
    this.updatePlayer();
    this.addAudioEventListeners();
    this.playAudio();
    this.increaseListenings();
  }


  updatePlayer(): void {
    let audioElement: HTMLAudioElement | null = null;
    if (this.audioElementRef) {
      audioElement = this.audioElementRef.nativeElement;
      // audioElement.pause();
      audioElement.src = this.audioUrl;
      audioElement.load();
    }

    if ('mediaSession' in navigator) {
      navigator.mediaSession.metadata = new MediaMetadata({
        title: this.title,
        artist: "Vladyslav Kovtun",
        artwork: [
          {src: this.audioUrl, sizes: '512x512', type: 'image/jpeg'},
        ],
      });
    }

    navigator.mediaSession.setActionHandler('previoustrack', () => {
      if (this.prevTrackId) {
        // this.router.navigate(['/soundtrack', this.prevTrackId], { queryParamsHandling: 'preserve' });
        window.location.href = '/soundtrack/' + this.prevTrackId;
      }
    });

    navigator.mediaSession.setActionHandler('nexttrack', () => {
      if (this.nextTrackId) {
        // console.log("MEDIA nexttrack")
        // audioElement?.pause();
        // this.router.navigate(['/soundtrack', this.nextTrackId], { queryParamsHandling: 'preserve' });
        window.location.href = '/soundtrack/' + this.nextTrackId;
      }
    });
    navigator.mediaSession.setActionHandler('play', () => audioElement?.play());
    navigator.mediaSession.setActionHandler('pause', () => audioElement?.pause());
  }

  ngOnDestroy(): void {
    if ('mediaSession' in navigator) {
      navigator.mediaSession.metadata = null;
    }
  }

  private addAudioEventListeners(): void {
    if (this.audioElementRef) {
      const audioElement = this.audioElementRef.nativeElement;
      audioElement.addEventListener('ended', this.onTrackEnded);
    }
  }

  private removeAudioEventListeners(): void {
    if (this.audioElementRef) {
      const audioElement = this.audioElementRef.nativeElement;
      audioElement.removeEventListener('ended', this.onTrackEnded);
    }
  }

  private playAudio(): void {
    if (this.audioElementRef) {
      const audioElement = this.audioElementRef.nativeElement;
      audioElement.play().catch((error) => {
        console.error("Error playing audio automatically:", error);
      });
    }
  }

  private onTrackEnded = (): void => {
    console.log("Track ended. Navigating to next track...");
    if (this.nextTrackId) {
      this.router.navigate(['/soundtrack', this.nextTrackId]);
    } else {
      console.warn("No next track ID provided.");
    }
  };

  private increaseListenings(): void {
    this.http.patch<any>(`${environment.apiUrl}/api/Soundtrack/${this.id}`, null).subscribe({
      next: (response) => {
      },
      error: (error) => {
        console.error('Error updating soundtrack:', error);
        alert(`Error: ${error.error?.error || 'Unable to update soundtrack.'}`);
      }
    });
  }
}
