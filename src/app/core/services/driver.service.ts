import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { DriverDashboard } from 'src/app/shared/types/driverDashboard.type';
import { DriverSettings } from 'src/app/shared/types/driverSettings.type';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  private baseUrl = environment.apiBaseUrl;
  constructor(private httpClient: HttpClient) {}

  getDashboardData(): Observable<DriverDashboard> {
    return this.httpClient.get<DriverDashboard>(
      `${this.baseUrl}/driver/dashboard`
    );
  }

  getDriverDetails(): Observable<DriverSettings> {
    return this.httpClient.get<DriverSettings>(
      `${this.baseUrl}/driver/settings`
    );
  }

  submitRating(driverId: number, rating: number) {
    return this.httpClient.post(`${this.baseUrl}/driver/rating/${driverId}`, rating);
  }
}
