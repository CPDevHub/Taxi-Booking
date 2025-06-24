import { Component, OnInit } from '@angular/core';
import { DriverService } from 'src/app/core/services/driver.service';
import { SignalrService } from 'src/app/core/services/signalrService.service';
import {
  DRIVER_AVAILABLE,
  DRIVER_UNAVAILABLE,
} from 'src/app/shared/constants/driver';
import { DriverSettings } from 'src/app/shared/types/driverSettings.type';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
})
export class DriverSettingsComponent implements OnInit {
  driver!: DriverSettings;
  constructor(
    private driverService: DriverService,
    private signalrService: SignalrService
  ) {}

  ngOnInit(): void {
    this.driverService.getDriverDetails().subscribe((data: DriverSettings) => {
      this.driver = data;
    });
  }

  onStatusToggle(): void {
    const newStatus =
      this.driver.status === DRIVER_AVAILABLE
        ? DRIVER_UNAVAILABLE
        : DRIVER_AVAILABLE;
    this.signalrService.updateStatus(newStatus);
    this.driver.status = newStatus;
  }
}
