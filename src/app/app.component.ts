import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from './core/services/signalrService.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'taxi-booking';

  constructor(private signalRService: SignalrService) {}

  ngOnInit(): void {
    this.signalRService.connect().catch(console.error);
  }

  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }
}
