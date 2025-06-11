import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { RideCancelType } from 'src/app/shared/types/rideCancel.type';
import { rideDetailsType } from 'src/app/shared/types/rideDetails.type';
import { RideRequestType } from 'src/app/shared/types/rideRequrest.type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class DriverHomeComponent implements OnInit {
  rideRequest: RideRequestType | null = null;
  rideDetails: rideDetailsType | null = null;
  showRideRequest = false;

  constructor(
    private signalRService: SignalrService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.signalRService.locationRequested$.subscribe(() => {
      navigator.geolocation.getCurrentPosition((position) => {
        this.signalRService.updateLocation(
          position.coords.latitude,
          position.coords.longitude
        );
      });
    });
    this.signalRService.rideAlreadyAccepted$.subscribe((data: number) => {
      this.showRideRequest = false;
      this.rideRequest = null;
    });

    this.signalRService.rideRequest$.subscribe(
      (ride: RideRequestType | null) => {
        if (ride) {
          this.showRideRequest = true;
          this.rideRequest = ride;
        } else {
          this.showRideRequest = false;
          this.rideRequest = null;
        }
      }
    );

    this.signalRService.rideCancelledByPassenger$.subscribe(
      (data: RideCancelType) => {
        this.showRideRequest = false;
        this.rideDetails = null;
        this.toaster.info(data.message);
      }
    );

    this.signalRService.rideAcceptDriverNotify$.subscribe(
      (data: rideDetailsType | null) => {
        this.rideDetails = data;
      }
    );
  }

  acceptRide() {
    if (this.rideRequest) this.signalRService.acceptRide(this.rideRequest.id);
  }
  rejectRide() {
    if (this.rideRequest) this.signalRService.rejectRide(this.rideRequest.id);
  }

  cancelRide() {
    if (this.rideDetails?.rideId) {
      const rideId = this.rideDetails.rideId;
      this.rideDetails = null;
      this.signalRService.cancelRideByDriver(rideId);
    }
  }
}
