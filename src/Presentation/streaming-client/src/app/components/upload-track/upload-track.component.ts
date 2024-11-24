import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../services/auth.service';
import { NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon'
import { Soundtrack } from '../../models/soundtrack';
import { finalize, Subscription } from 'rxjs';
import { MatProgressBar } from '@angular/material/progress-bar';

@Component({
  selector: 'app-upload-track',
  standalone: true,
  imports: [NgIf, MatIconModule, MatProgressBar],
  templateUrl: './upload-track.component.html',
  styleUrl: './upload-track.component.css'
})
export class UploadTrackComponent {
  selectedFile: File | null = null;
  errorMessage: string = '';
  @Output() updateList = new EventEmitter<void>();
  fileName = '';
  uploadProgress: number | null = null;
  uploadSub: Subscription | null = null;

  constructor(private http: HttpClient) { }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.fileName = file.name;
    }
  }

  uploadSoundtrack(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    const upload$ = this.http.post<any>(`${environment.apiUrl}/api/Soundtrack`, formData, {
      reportProgress: true,
      observe: 'events'
    }).pipe(
      finalize(() => this.reset())
    );

    this.uploadSub = upload$.subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          this.uploadProgress = Math.round((100 * event.loaded) / event.total);
        } else if (event.type === HttpEventType.Response && event.body.isSuccess) {
          this.updateList.emit();
        }
      },
      error: (error) => {
        console.log(error)
        this.errorMessage = error.status === 400 && error.error?.error
          ? error.error.error
          : 'An unexpected error occurred. Please try again.';
      }
    });
  }

  cancelUpload(): void {
    if (this.uploadSub) {
      this.uploadSub.unsubscribe();
      this.reset();
    }
  }

  reset(): void {
    this.uploadProgress = null;
    this.uploadSub = null;
    this.selectedFile = null;
    this.fileName = '';
  }
}
