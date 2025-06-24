import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RideService } from '../../services/ride.service';
import { Router } from '@angular/router';
import { RideHistoryType } from 'src/app/shared/types/rideHistory.type';
import { DRIVER, PASSENGER } from 'src/app/shared/constants/roles';

@Component({
  selector: 'app-ride-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
})
export class RideHistoryComponent implements OnInit {
  rideHistory: RideHistoryType[] = [];
  role: 'Driver' | 'Passenger' = 'Driver';

  constructor(private rideService: RideService, private router: Router) {}

  ngOnInit(): void {
    const url = this.router.url;
    if (url.includes(`/${DRIVER.toLocaleLowerCase()}/`)) {
      this.role = DRIVER;
      this.getRideHistoryDriver();
    } else if (url.includes(`/${PASSENGER.toLocaleLowerCase()}/`)) {
      this.role = PASSENGER;
      this.getRideHistoryPassenger();
    }
  }

  getRideHistoryDriver() {
    this.rideService
      .getRideHistoryDriver()
      .subscribe((data: RideHistoryType[]) => {
        this.rideHistory = data;
      });
  }

  getRideHistoryPassenger() {
    this.rideService
      .getRideHistoryPassenger()
      .subscribe((data: RideHistoryType[]) => {
        this.rideHistory = data;
      });
  }
}
