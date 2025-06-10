import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { rideAcceptType } from 'src/app/shared/types/rideAccept.type';
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

  private rideAcceptSubject = new Subject<rideAcceptType | null>();
  rideAccept$ = this.rideAcceptSubject.asObservable();

  private rideAlreadyAcceptedSubject = new Subject<number>();
  rideAlreadyAccepted$ = this.rideAlreadyAcceptedSubject.asObservable();

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
      this.rideRequestSubject.next(ride);
    });

    this.hubConnection.on('RideAccepted', (data: any) => {
      this.rideAcceptSubject.next(data);
    });

    this.hubConnection.on('RideAlreadyAccepted', (data) => {
      this.rideAlreadyAcceptedSubject.next(data);
    });
  }

  async updateLocation(lat: number, lng: number) {
    await this.hubConnection.invoke('UpdateLocation', {
      latitude: lat,
      longitude: lng,
    });
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
