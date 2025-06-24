import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import {
  ApiDashboard,
  ApiDriverSettings,
  Apilocation,
  ApiSubmitRating,
} from 'src/app/shared/constants/routes';
import { DriverDashboard } from 'src/app/shared/types/driverDashboard.type';
import { DriverSettings } from 'src/app/shared/types/driverSettings.type';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  constructor(private httpClient: HttpClient) {}

  getDashboardData(): Observable<DriverDashboard> {
    return this.httpClient.get<DriverDashboard>(ApiDashboard);
  }

  getDriverDetails(): Observable<DriverSettings> {
    return this.httpClient.get<DriverSettings>(ApiDriverSettings);
  }

  submitRating(driverId: number, rating: number) {
    return this.httpClient.post(`${ApiSubmitRating}/${driverId}`, rating);
  }

  getCurrentLocation(driverId: number) {
    return this.httpClient.get(`${Apilocation}/${driverId}`);
  }
}
