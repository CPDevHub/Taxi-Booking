import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { RideCancelType } from 'src/app/shared/types/rideCancel.type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class PassengerHomeComponent implements OnInit {
  rideDetails: rideAcceptType | null = null;
  cancelRideModal: boolean = false;

  pickupCoords: any;
  dropoffCoords: any;

  handlePickupSelected(location: any) {
    this.pickupCoords = location;
    console.log(location);
  }

  handleDropoffSelected(location: any) {
    this.dropoffCoords = location;
    console.log(location);
  }

  constructor(
    private signalr: SignalrService,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.signalr.rideAcceptPassengerNotify$.subscribe(
      (data: rideAcceptType | null) => {
        this.rideDetails = data;
      }
    );

    this.signalr.rideCancelledByDriver$.subscribe((data: RideCancelType) => {
      this.toaster.info(data.message);
      this.rideDetails = null;
    });

    this.signalr.rideCompleted$.subscribe(() => {
      console.log('Ride completed');
      this.rideDetails=null;
    });

    this.signalr.rideStart$.subscribe(() => {
      console.log('ride started');
      this.rideDetails=null;
    });
  }

  rideCancelConfirm(data: { rideId: number; reason: string }) {
    this.rideDetails = null;
    this.signalr.cancelRideByPassenger(data);
  }

  onCancelRide() {
    this.cancelRideModal = true;
  }

  closeModal() {
    this.cancelRideModal = false;
  }
}
