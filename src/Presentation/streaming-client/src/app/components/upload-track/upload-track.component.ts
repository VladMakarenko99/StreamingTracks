import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { environment } from '../../../enviroments/enviroment';
import { AuthService } from '../../services/auth.service';
import { NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon'
import { Soundtrack } from '../../models/soundtrack';

@Component({
  selector: 'app-upload-track',
  standalone: true,
  imports: [NgIf, MatIconModule],
  templateUrl: './upload-track.component.html',
  styleUrl: './upload-track.component.css'
})
export class UploadTrackComponent {
  selectedFile: File | null = null;
  errorMessage: string = '';
  isAdmin: boolean = false;
  @Output() updateList = new EventEmitter<void>();

  constructor(private http: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    this.isAdmin = this.authService.getUser()?.role == 'Admin'
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] || null;
  }

  uploadSoundtrack(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post<any>(`${environment.apiUrl}/api/Soundtrack`, formData).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.errorMessage = '';
          this.updateList.emit();
        }
      },
      error: (error) => {
        if (error.status === 400 && error.error && error.error.error) {
          this.errorMessage = error.error.error;
        } else {
          this.errorMessage = 'An unexpected error occurred. Please try again.';
        }
      }
    });
  }
}
