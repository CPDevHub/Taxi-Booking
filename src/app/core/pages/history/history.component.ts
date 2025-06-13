import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RideService } from '../../services/ride.service';
import { Router } from '@angular/router';
import { RideHistoryType } from 'src/app/shared/types/rideHistory.type';

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
    console.log(url)
    if (url.includes('/driver/')) {
      this.role = 'Driver';
      this.getRideHistoryDriver();
    } else if (url.includes('/passenger/')) {
      this.role = 'Passenger';
      this.getRideHistoryPassenger();
    }
  }

  getRideHistoryDriver() {
    this.rideService
      .getRideHistoryDriver()
      .subscribe((data: RideHistoryType[]) => {
        console.log(data);
        this.rideHistory = data;
      });
  }

  getRideHistoryPassenger() {
    this.rideService
      .getRideHistoryPassenger()
      .subscribe((data: RideHistoryType[]) => {
        console.log(data);
        this.rideHistory = data;
      });
  }
}
