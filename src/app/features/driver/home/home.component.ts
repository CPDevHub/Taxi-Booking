import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MapComponent } from 'src/app/core/components/map/map.component';
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
@ViewChild(MapComponent) mapComponent!:MapComponent


  rideRequests: RideRequestType[] = [];
  rideDetails: rideDetailsType | null = null;
  driverCoords!: any;
  rideStarted:boolean=false;
  timerId!: any;

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
      this.rideRequests = this.rideRequests.filter((ride) => ride.id !== data);
    });

    this.signalRService.rideRequest$.subscribe((ride: RideRequestType) => {
      this.rideRequests.push(ride);
    });

    this.signalRService.rideCancelledByPassengerBeforeAccept$.subscribe(
      (data: RideCancelType) => {
        this.rideRequests = this.rideRequests.filter(
          (ride) => ride.id !== data.rideId
        );
      }
    );

    this.signalRService.rideCancelledByPassenger$.subscribe(
      (data: RideCancelType) => {
        this.rideDetails = null;
        clearInterval(this.timerId);
        this.mapComponent.resetMap();
        this.driverCoords = null;
        this.rideStarted=false;
        this.toaster.info(data.message);
      }
    );

    this.signalRService.rideAcceptDriverNotify$.subscribe(
      (data: rideDetailsType | null) => {
        this.rideDetails = data;
        this.rideStarted=false;
        this.rideRequests = [];
        this.timerId = setInterval(() => {
          window.navigator.geolocation.getCurrentPosition((pos) => {
            this.driverCoords = pos.coords;
          });
        }, 3000);
      }
    );
  }

  acceptRide(id: number) {
    this.signalRService.acceptRide(id);
  }

  rejectRide(id: number) {
    this.signalRService.rejectRide(id);
    this.rideRequests = this.rideRequests.filter((ride) => ride.id !== id);
  }

  cancelRide() {
    if (this.rideDetails?.rideId) {
      const rideId = this.rideDetails.rideId;
      this.rideDetails = null;
      this.rideRequests = [];
      clearInterval(this.timerId);
      this.driverCoords = null;
      this.mapComponent.resetMap();
      this.signalRService.cancelRideByDriver(rideId);
    }
  }

  completeRide() {
    if (this.rideDetails?.rideId) {
      const rideId = this.rideDetails.rideId;
      this.rideDetails = null;
      clearInterval(this.timerId);
      this.driverCoords = null;
      this.rideRequests = [];
      this.mapComponent.resetMap();
      this.signalRService.completeRide(rideId);
    }
  }

  startRide() {
    if (this.rideDetails?.rideId) {
      const rideId = this.rideDetails.rideId;
      clearInterval(this.timerId);
      this.rideStarted=true;
      this.driverCoords = null;
      this.signalRService.startRide(rideId);
    }
  }
}
