import { Component, OnInit } from '@angular/core';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { RideRequestType } from 'src/app/shared/types/rideRequrest.type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class DriverHomeComponent implements OnInit {
  rideRequest: RideRequestType | null = null;
  showRideRequest = false;

  constructor(private signalRService: SignalrService) {}

  ngOnInit(): void {
    this.signalRService.locationRequested$.subscribe(() => {
      navigator.geolocation.getCurrentPosition((position) => {
        this.signalRService.updateLocation(
          position.coords.latitude,
          position.coords.longitude
        );
      });

      this.signalRService.rideAlreadyAccepted$.subscribe((data) => {
        this.showRideRequest = false;
        this.rideRequest = null;
      });

      this.signalRService.rideRequest$.subscribe((ride) => {
        if (ride) {
          this.showRideRequest = true;
          this.rideRequest = ride;
        } else {
          this.showRideRequest = false;
          this.rideRequest = null;
        }
      });
    });
  }

  acceptRide() {
    if (this.rideRequest) this.signalRService.acceptRide(this.rideRequest.id);
  }
  rejectRide() {
    if (this.rideRequest) this.signalRService.rejectRide(this.rideRequest.id);
  }
}
