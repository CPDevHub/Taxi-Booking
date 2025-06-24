import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import {
  AcceptRide,
  CancelRideBeforeAcceptance,
  CancelRideByDriver,
  CancelRideByPassenger,
  CompleteRide,
  LoginDriver,
  LoginPassenger,
  ReceiveRideRequest,
  RejectRide,
  RideAcceptedDriverNotify,
  RideAcceptedUserNotify,
  RideAlreadyAccepted,
  RideCancelledByDriver,
  RideCancelledByPassenger,
  RideCancelledByPassengerBeforeAccept,
  RideCompleted,
  RideStart,
  SendLocation,
  UpdateLocation,
  UpdateStatus,
} from 'src/app/shared/constants/SignalREvents';
import { ACCESS_TOKEN } from 'src/app/shared/constants/token';
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

  private rideRequestSubject = new Subject<RideRequestType>();
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

  private rideCancelledByPassengerBeforeAcceptSubject =
    new Subject<RideCancelType>();
  rideCancelledByPassengerBeforeAccept$ =
    this.rideCancelledByPassengerBeforeAcceptSubject.asObservable();

  private rideCompletedSubject = new Subject<void>();
  rideCompleted$ = this.rideCompletedSubject.asObservable();

  private rideStartSubject = new Subject<void>();
  rideStart$ = this.rideStartSubject.asObservable();

  connect(): void {
    const token = sessionStorage.getItem(ACCESS_TOKEN);
    if (!token) return;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7134/taxiBookingHub', {
        accessTokenFactory: () => {
          return sessionStorage.getItem(ACCESS_TOKEN) || '';
        },
      })
      .withAutomaticReconnect()
      .build();

    this.registerListeners();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch((err) => console.error('Error connecting to SignalR', err));
  }

  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .then(() => {
          console.log('SignalR connection stopped');
        })
        .catch((err) => console.error('Error stopping connection:', err));
    }
  }

  private registerListeners() {
    this.hubConnection.on(SendLocation, () => {
      this.locationRequestedSubject.next();
    });

    this.hubConnection.on(ReceiveRideRequest, (ride: RideRequestType) => {
      this.rideRequestSubject.next(ride);
    });

    this.hubConnection.on(RideAcceptedUserNotify, (data: rideAcceptType) => {
      this.rideAcceptPassengerNotifySubject.next(data);
    });

    this.hubConnection.on(RideStart, () => {
      this.rideStartSubject.next();
    });

    this.hubConnection.on(RideCompleted, () => {
      this.rideCompletedSubject.next();
    });

    this.hubConnection.on(RideAcceptedDriverNotify, (data: rideDetailsType) => {
      this.rideAcceptDriverNotifySubject.next(data);
    });

    this.hubConnection.on(RideAlreadyAccepted, (data: { rideId: number }) => {
      this.rideAlreadyAcceptedSubject.next(data.rideId);
    });

    this.hubConnection.on(RideCancelledByDriver, (data: RideCancelType) => {
      this.rideCancelledByDriverSubject.next(data);
    });

    this.hubConnection.on(RideCancelledByPassenger, (data: RideCancelType) => {
      this.rideCancelledByPassengerSubject.next(data);
    });

    this.hubConnection.on(
      RideCancelledByPassengerBeforeAccept,
      (data: RideCancelType) => {
        this.rideCancelledByPassengerBeforeAcceptSubject.next(data);
      }
    );
  }

  async updateLocation(lat: number, lng: number) {
    await this.hubConnection.invoke(UpdateLocation, {
      latitude: lat,
      longitude: lng,
    });
  }

  async cancelRideByDriver(rideId: number) {
    await this.hubConnection.invoke(CancelRideByDriver, rideId);
  }

  async completeRide(rideId: number) {
    await this.hubConnection.invoke(CompleteRide, rideId);
  }
  async startRide(rideId: number) {
    await this.hubConnection.invoke(RideStart, rideId);
  }

  async cancelRideByPassenger(data: { rideId: number; reason: string }) {
    console.log(data.rideId, data.reason);
    await this.hubConnection.invoke(
      CancelRideByPassenger,
      data.rideId,
      data.reason
    );
  }

  async loginDriver(driverId: number) {
    await this.hubConnection.invoke(LoginDriver, driverId);
  }

  async loginPassenger(passengerId: number) {
    await this.hubConnection.invoke(LoginPassenger, passengerId);
  }

  async acceptRide(rideId: number) {
    await this.hubConnection.invoke(AcceptRide, rideId);
  }

  async rejectRide(rideId: number) {
    await this.hubConnection.invoke(RejectRide, rideId);
  }
  async cancelRideBeforeAcceptance(rideId: number) {
    this.hubConnection.invoke(CancelRideBeforeAcceptance, rideId);
  }

  updateStatus(status: 'Available' | 'Unavailable') {
    this.hubConnection.invoke<void>(UpdateStatus, status);
  }
}
