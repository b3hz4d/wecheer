import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, map } from 'rxjs';
import { ImageEvent } from '../models/image-event';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageEventService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLatestEvent(): Observable<ImageEvent> {
    return this.http.get<ImageEvent>(`${this.apiUrl}/api/ImageEvent/latest`).pipe(
      tap(response => console.log('Latest event response:', response))
    );
  }

  getEventCountLastHour(): Observable<number> {
    return this.http.get<{ count: number }>(`${this.apiUrl}/api/ImageEvent/count`).pipe(
      map(response => response.count),
      tap(count => console.log('Event count response:', count))
    );
  }
} 