import { Component } from '@angular/core';
import { SoundtrackListComponent } from '../soundtrack-list/soundtrack-list.component';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SoundtrackListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
