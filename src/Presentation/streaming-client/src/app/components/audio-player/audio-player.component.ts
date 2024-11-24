import {environment} from "../../../environments/environment";
import {Component, Input, OnInit, AfterViewInit, OnDestroy} from "@angular/core";

@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [],
  templateUrl: './audio-player.component.html',
  styleUrl: './audio-player.component.css'
})
export class AudioPlayerComponent implements OnInit {
  @Input() id: string = '';
  imageUrl: string = '';
  @Input() albumCoverName: string = '';
  audioUrl: string = '';
  @Input() title: string = 'Default Title';
  private audioElement!: HTMLAudioElement;

  ngOnInit(): void {
    this.audioUrl = `${environment.apiUrl}/api/Streaming/` + this.id;
    if(this.albumCoverName){
      this.imageUrl = `${environment.apiUrl}/api/Soundtrack/image/${this.albumCoverName}`;
    }
    else{
      this.imageUrl = "album.png";
    }

    this.audioElement = document.querySelector('audio#audio-' + this.id)!;
    if ('mediaSession' in navigator) {
      navigator.mediaSession.metadata = new MediaMetadata({
        title: this.title,
        artist: "Vladyslav Kovtun",
        artwork: [
          { src: this.imageUrl, sizes: '512x512', type: 'image/jpeg' },
        ],
      });
    }
  }

  ngOnDestroy() {
    if ('mediaSession' in navigator) {
      navigator.mediaSession.metadata = null;
    }
  }
}
