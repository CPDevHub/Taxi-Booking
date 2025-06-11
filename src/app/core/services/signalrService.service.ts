import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
import { RideCancelType } from 'src/app/shared/types/rideCancel.type';
import { rideDetailsType } from 'src/app/shared/types/rideDetails.type';
import { RideRequestType } from 'src/app/shared/types/rideRequrest.type';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection!: HubConnection;

  private locationRequestedSubject = new Subject<void>();
  locationRequested$ = this.locationRequestedSubject.asObservable();

  private rideRequestSubject = new Subject<RideRequestType | null>();
  rideRequest$ = this.rideRequestSubject.asObservable();

  private rideAcceptPassengerNotifySubject =
    new Subject<rideAcceptType | null>();
  rideAcceptPassengerNotify$ =
    this.rideAcceptPassengerNotifySubject.asObservable();

  private rideAcceptDriverNotifySubject = new Subject<rideDetailsType | null>();
  rideAcceptDriverNotify$ = this.rideAcceptDriverNotifySubject.asObservable();

  private rideAlreadyAcceptedSubject = new Subject<number>();
  rideAlreadyAccepted$ = this.rideAlreadyAcceptedSubject.asObservable();

  private rideCancelledByDriverSubject = new Subject<RideCancelType>();
  rideCancelledByDriver$ = this.rideCancelledByDriverSubject.asObservable();

  private rideCancelledByPassengerSubject = new Subject<RideCancelType>();
  rideCancelledByPassenger$ =
    this.rideCancelledByPassengerSubject.asObservable();

  connect(): Promise<void> {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7134/taxiBookingHub', {
        accessTokenFactory: () => {
          return sessionStorage.getItem('access_token') || '';
        },
      })
      .withAutomaticReconnect()
      .build();

    this.registerListeners();

    return this.hubConnection.start();
  }

  private registerListeners() {
    this.hubConnection.on('SendLocation', () => {
      this.locationRequestedSubject.next();
    });

    this.hubConnection.on('ReceiveRideRequest', (ride: RideRequestType) => {
      console.log('Ride request received:', ride);
      this.rideRequestSubject.next(ride);
    });

    this.hubConnection.on('RideAcceptedUserNotify', (data: rideAcceptType) => {
      console.log('Ride accepted:', data);
      this.rideAcceptPassengerNotifySubject.next(data);
    });

    this.hubConnection.on(
      'RideAcceptedDriverNotify',
      (data: rideDetailsType) => {
        console.log('Ride accepted:', data);
        this.rideAcceptDriverNotifySubject.next(data);
      }
    );

    this.hubConnection.on('RideAlreadyAccepted', (data: number) => {
      console.log('Ride already accepted:', data);
      this.rideAlreadyAcceptedSubject.next(data);
    });

    this.hubConnection.on('RideCancelledByDriver', (data: RideCancelType) => {
      console.log('Ride cancelled by driver');
      this.rideCancelledByDriverSubject.next(data);
    });

    this.hubConnection.on(
      'RideCancelledByPassenger',
      (data: RideCancelType) => {
        console.log('Ride cancelled by Passenger');
        this.rideCancelledByPassengerSubject.next(data);
      }
    );
  }

  async updateLocation(lat: number, lng: number) {
    await this.hubConnection.invoke('UpdateLocation', {
      latitude: lat,
      longitude: lng,
    });
  }

  async cancelRideByDriver(rideId: number) {
    await this.hubConnection.invoke('CancelRideByDriver', rideId);
  }

  async cancelRideByPassenger(data: { rideId: number; reason: string }) {
    await this.hubConnection.invoke(
      'CancelRideByPassenger',
      data.rideId,
      data.reason
    );
  }

  async loginDriver(driverId: number) {
    await this.hubConnection.invoke('LoginDriver', driverId);
  }

  async loginPassenger(passengerId: number) {
    await this.hubConnection.invoke('LoginPassenger', passengerId);
  }

  async acceptRide(rideId: number) {
    await this.hubConnection.invoke('AcceptRide', rideId);
    this.rideRequestSubject.next(null);
  }

  async rejectRide(rideId: number) {
    await this.hubConnection.invoke('RejectRide', rideId);
    this.rideRequestSubject.next(null);
  }
}
