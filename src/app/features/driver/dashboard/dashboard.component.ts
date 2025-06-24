import { Component, OnInit } from '@angular/core';
import { DriverService } from 'src/app/core/services/driver.service';
import { DriverDashboard } from 'src/app/shared/types/driverDashboard.type';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  driver!: DriverDashboard;
  constructor(private driverService: DriverService) {}

  ngOnInit(): void {
    this.driverService.getDashboardData().subscribe((data: DriverDashboard) => {
      this.driver = data;
    });
  }
}
