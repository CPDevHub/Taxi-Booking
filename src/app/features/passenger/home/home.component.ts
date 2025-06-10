import { Component, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class PassengerHomeComponent implements OnInit {
  rideDetails: rideAcceptType | null = null;
  constructor(private signalr: SignalrService) {}

  ngOnInit(): void {
    this.signalr.rideAccept$.subscribe((data) => {
      this.rideDetails = data;
    });
  }
}
