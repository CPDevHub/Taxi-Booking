import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Driver } from 'src/app/shared/types/driver.type';
import { PassengerType } from 'src/app/shared/types/passenger.type';
import { environment } from 'src/environments/environment';
import { LoginCredentialsType } from 'src/app/shared/types/loginCredentials.type';
import {
  ApiDriverLogin,
  ApiDriverSignup,
  ApiIsAuthenticated,
  ApiPassengerLogin,
  ApiPassengerSignup,
} from 'src/app/shared/constants/routes';
import { DRIVER, PASSENGER } from 'src/app/shared/constants/roles';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  login(data: LoginCredentialsType, userType: string) {
    const loginUrl =
      userType === PASSENGER.toLocaleLowerCase()
        ? `${ApiPassengerLogin}`
        : `${ApiDriverLogin}`;
    return this.httpClient.post(loginUrl, data);
  }

  driverSignup(driver: Driver) {
    console.log(driver);
    return this.httpClient.post(ApiDriverSignup, driver);
  }

  passengerSignup(passenger: PassengerType) {
    return this.httpClient.post(ApiPassengerSignup, passenger);
  }

  isLoggedIn() {
    return this.httpClient.get<{ isAuthenticated: boolean; role?: string }>(
      ApiIsAuthenticated
    );
  }
}
