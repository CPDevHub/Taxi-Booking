import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { rideBookType } from 'src/app/shared/types/rideBook.type';
import { RideBookResponseType } from 'src/app/shared/types/rideBookResponse.type';
import { RideHistoryType } from 'src/app/shared/types/rideHistory.type';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RideService {
  baseUrl = environment.apiBaseUrl;
  constructor(private httpClient: HttpClient) {}

  bookRide(ride: rideBookType) {
    return this.httpClient.post<RideBookResponseType>(
      `${this.baseUrl}/ride/book`,
      ride
    );
  }

  getRideHistoryDriver() {
    return this.httpClient.get<RideHistoryType[]>(
      `${this.baseUrl}/driver/history`
    );
  }

  getRideHistoryPassenger() {
    return this.httpClient.get<RideHistoryType[]>(
      `${this.baseUrl}/passenger/history`
    );
  }
}
