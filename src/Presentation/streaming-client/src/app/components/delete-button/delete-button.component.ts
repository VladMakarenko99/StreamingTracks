import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { environment } from '../../../enviroments/enviroment';

@Component({
  selector: 'app-delete-button',
  standalone: true,
  imports: [],
  templateUrl: './delete-button.component.html',
  styleUrl: './delete-button.component.css'
})
export class DeleteButtonComponent {
  @Input() soundtrackId: string = "";
  @Output() onDeleted = new EventEmitter<void>();

  constructor(private http: HttpClient) {}

  deleteSoundtrack() {
    this.http.delete<any>(`${environment.apiUrl}/api/Soundtrack?Id=${this.soundtrackId}`).subscribe({
      next: (response) => {
        if(response.isSuccess){
          alert(response.body);
          this.onDeleted.emit();
        }
      },
      error: (error) => {
        console.error('Error deleting soundtrack:', error);
        alert(`Error: ${error.error.error || 'Unable to delete soundtrack.'}`);
      }
    });
  }
}
