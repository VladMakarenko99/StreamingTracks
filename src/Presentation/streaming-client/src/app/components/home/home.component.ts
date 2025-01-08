import {Component, OnInit} from '@angular/core';
import { SoundtrackListComponent } from '../soundtrack-list/soundtrack-list.component';
import {Meta, Title} from "@angular/platform-browser";


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SoundtrackListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  constructor(private meta: Meta, private title: Title) {}

  ngOnInit(): void {
    this.title.setTitle('Home Page');
    this.meta.addTags([
      { name: 'description', content: 'This is a storage for every art work of Vladyslav Kovtun including covers, songs and soundtracks.' },
      { name: 'keywords', content: 'Soundtracks, Vladyslav Kovtun, Songs, Art' },
      { name: 'author', content: 'Vladyslav Kovtun' }
    ]);
  }
}
