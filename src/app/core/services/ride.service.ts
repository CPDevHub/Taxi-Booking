import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  ApiBookRide,
  ApiDriverHistory,
  ApiPassengerHistory,
} from 'src/app/shared/constants/routes';
import { rideBookType } from 'src/app/shared/types/rideBook.type';
import { RideBookResponseType } from 'src/app/shared/types/rideBookResponse.type';
import { RideHistoryType } from 'src/app/shared/types/rideHistory.type';

@Injectable({
  providedIn: 'root',
})
export class RideService {
  constructor(private httpClient: HttpClient) {}

  bookRide(ride: rideBookType) {
    return this.httpClient.post<RideBookResponseType>(ApiBookRide, ride);
  }

  getRideHistoryDriver() {
    return this.httpClient.get<RideHistoryType[]>(ApiDriverHistory);
  }

  getRideHistoryPassenger() {
    return this.httpClient.get<RideHistoryType[]>(ApiPassengerHistory);
  }
}
