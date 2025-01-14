import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { interval, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

import { ImageEvent } from './models/image-event';
import { ImageEventService } from './services/image-event.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  latestEvent: ImageEvent | null = null;
  eventCount: number = 0;
  private updateSubscription?: Subscription;

  constructor(private imageEventService: ImageEventService) {}

  ngOnInit() {
    // Initial data fetch
    this.fetchLatestEvent();
    this.fetchEventCount();

    // Set up polling every 5 seconds
    this.updateSubscription = interval(5000).subscribe(() => {
      this.fetchLatestEvent();
      this.fetchEventCount();
    });
  }

  ngOnDestroy() {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }

  private fetchLatestEvent() {
    this.imageEventService.getLatestEvent().subscribe({
      next: (event) => {
        console.log('Processing latest event:', event);
        this.latestEvent = event;
      },
      error: (error) => {
        console.error('Error fetching latest event:', error);
      }
    });
  }

  private fetchEventCount() {
    this.imageEventService.getEventCountLastHour().subscribe({
      next: (count) => {
        console.log('Processing event count:', count);
        // Ensure we're getting a number
        this.eventCount = typeof count === 'number' ? count : parseInt(count as any, 10) || 0;
      },
      error: (error) => {
        console.error('Error fetching event count:', error);
      }
    });
  }
}
