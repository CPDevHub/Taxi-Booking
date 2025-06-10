import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Driver } from 'src/app/shared/types/driver.type';
import { Passenger } from 'src/app/shared/types/passenger.type';
import { environment } from 'src/environments/environment';
import { LoginCredentials } from 'src/app/shared/types/loginCredentials.type';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiBaseUrl;
  constructor(private httpClient: HttpClient) {}

  login(data: LoginCredentials, userType: string) {
    return this.httpClient.post(`${this.baseUrl}/auth/${userType}-login`, data);
  }

  driverSignup(driver: Driver) {
    return this.httpClient.post(`${this.baseUrl}/auth/driver-signup`, driver);
  }
  passengerSignup(passenger: Passenger) {
    return this.httpClient.post(
      `${this.baseUrl}/auth/passenger-signup`,
      passenger
    );
  }

  isLoggedIn() {
    return this.httpClient.get<{ isAuthenticated: boolean; role?: string }>(
      `${this.baseUrl}/auth/is-authenticated`
    );
  }
}
