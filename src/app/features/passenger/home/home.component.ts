import { asNativeElements, Component, OnInit, ViewChild } from '@angular/core';
import { AnyARecord } from 'dns';
import { ToastrService } from 'ngx-toastr';
import { MapComponent } from 'src/app/core/components/map/map.component';
import { DriverService } from 'src/app/core/services/driver.service';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { RideCancelType } from 'src/app/shared/types/rideCancel.type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class PassengerHomeComponent implements OnInit {
  @ViewChild(MapComponent) mapComponent!: MapComponent;
  rideDetails: rideAcceptType | null = null;
  cancelRideModal: boolean = false;
  rideStarted: boolean = false;

  pickupCoords: any;
  dropoffCoords: any;
  driverCoords: any;

  timerId!: any;

  handlePickupSelected(location: any) {
    this.pickupCoords = {
      latitude: location.latitude,
      longitude: location.longitude,
    };
  }

  handleDropoffSelected(location: any) {
    this.dropoffCoords = {
      latitude: location.latitude,
      longitude: location.longitude,
    };
  }

  constructor(
    private signalr: SignalrService,
    private toaster: ToastrService,
    private driverService: DriverService
  ) {}

  ngOnInit(): void {
    this.signalr.rideAcceptPassengerNotify$.subscribe(
      (data: rideAcceptType | null) => {
        this.rideDetails = data;
        this.timerId = setInterval(() => {
          if (this.rideDetails) {
            this.driverService
              .getCurrentLocation(this.rideDetails?.driverId)
              .subscribe((data: any) => {
                this.driverCoords = data;
              });
          }
        }, 3000);
      }
    );

    this.signalr.rideCancelledByDriver$.subscribe((data: RideCancelType) => {
      this.toaster.info(data.message);
      this.driverCoords = null;
      this.rideStarted = false;
      this.rideDetails = null;
      clearInterval(this.timerId);
    });

    this.signalr.rideStart$.subscribe(() => {
      console.log('ride started');
      this.driverCoords = null;
      this.rideStarted = true;
      clearInterval(this.timerId);
    });
  }

  rideCancelConfirm(data: { rideId: number; reason: string }) {
    this.rideDetails = null;
    this.driverCoords = null;
    this.mapComponent.resetMap();
    this.signalr.cancelRideByPassenger(data);
  }

  onCancelRide() {
    this.cancelRideModal = true;
  }

  closeModal() {
    this.cancelRideModal = false;
  }

  rideCompleted() {
    this.rideDetails = null;
    this.driverCoords = null;
    this.pickupCoords = null;
    this.dropoffCoords = null;
    this.mapComponent.resetMap();
  }
}
