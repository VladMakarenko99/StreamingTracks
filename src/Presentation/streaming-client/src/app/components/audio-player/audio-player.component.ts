import {environment} from "../../../enviroments/enviroment";
import {Component, Input, OnInit} from "@angular/core";

@Component({
  selector: 'app-audio-player',
  standalone: true,
  imports: [],
  templateUrl: './audio-player.component.html',
  styleUrl: './audio-player.component.css'
})
export class AudioPlayerComponent implements OnInit {
  @Input() id: string = '';
  audioUrl: string = '';
  @Input() title: string = 'Default Title';

  ngOnInit(): void {
    this.audioUrl = `${environment.apiUrl}/api/Streaming/` + this.id;
  }
}
